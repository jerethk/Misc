package com.example.spasmometer;

import androidx.appcompat.app.AppCompatActivity;
import android.hardware.Sensor;
import android.hardware.SensorEvent;
import android.hardware.SensorEventListener;
import android.hardware.SensorManager;
import android.os.Bundle;
import android.os.Handler;
import android.os.Looper;
import android.os.PowerManager;
import android.view.View;
import android.widget.Button;
import android.widget.TextView;
import java.util.ArrayList;

public class MainActivity extends AppCompatActivity {
    private SpasmometerModel model;

    private TextView txtTime;
    private TextView outputText;
    private Button startButton;
    private Button stopButton;

    private SensorManager sensorManager;
    private Sensor accelSensor;
    private SensorEventListener accelListener;

    private PowerManager.WakeLock wakelock;

    // CONSTANTS //
    private static final int MAX_TIME = 5400;   // seconds
    public static final int WAKELOCK_TIMEOUT = (MAX_TIME + 60) * 1000;  // milliseconds
    private static final float SENSOR_THRESHOLD = 0.9f;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        model = new SpasmometerModel();

        txtTime = findViewById(R.id.txtTime);
        startButton = findViewById(R.id.btnStart);
        stopButton = findViewById(R.id.btnStop);
        outputText = findViewById(R.id.outputText);
        //testbutton = findViewById(R.id.button3);
        stopButton.setEnabled(false);

        // Initialise sensor
        sensorManager = (SensorManager) getSystemService(SENSOR_SERVICE);
        accelSensor = sensorManager.getDefaultSensor(Sensor.TYPE_LINEAR_ACCELERATION);
        accelListener = new SensorEventListener() {
            @Override
            public void onSensorChanged(SensorEvent e) {
                double absoluteAccel = Math.sqrt(e.values[0] * e.values[0] +
                        e.values[1] * e.values[1] +
                        e.values[2] * e.values[2]);

                if (absoluteAccel > SENSOR_THRESHOLD) {
                    model.newEvent(absoluteAccel);
                    updateOutput();
                }
            }

            @Override
            public void onAccuracyChanged(Sensor sensor, int i) {
                // do nothing
            }
        };

        /*   // determine whether sensor is wakeup or not
        boolean b = accelSensor.isWakeUpSensor();
        outputText.setText(Boolean.toString(b));    // my phone's result: false
        */

        // Initialise wakelock
        PowerManager powerManager = (PowerManager) getSystemService(POWER_SERVICE);
        wakelock = powerManager.newWakeLock(PowerManager.PARTIAL_WAKE_LOCK, "app:myWakelock");

        outputText.setText("Timeout " + Integer.toString(WAKELOCK_TIMEOUT));
    }

    public void btnStartClick(View view) {
        model.startTimer();
        startButton.setEnabled(false);
        stopButton.setEnabled(true);
        outputText.setText(" ");

        sensorManager.registerListener(accelListener, accelSensor, 100000/*SensorManager.SENSOR_DELAY_NORMAL*/);
        wakelock.acquire(WAKELOCK_TIMEOUT);

        // start the service which runs the detector
        //Intent intent = new Intent(this, DetectorService.class);
        //startService(intent);

        // start the timer, on another thread
        Thread timerThread = new Thread(new Timer());
        timerThread.start();
    }

    public void btnStopClick(View view) {
        stopRecording();
    }

    private void stopRecording() {
        long totalTime = model.stopTimer();
        int minutes = (int) totalTime / 60;
        int seconds = (int) totalTime % 60;
        String totTime = minutes + " m " + seconds + " s";
        txtTime.setText(totTime);

        model.saveRecording(this);
        sensorManager.unregisterListener(accelListener);
        //wakelock.release();   // a timeout is set above so release is not necessary

        startButton.setEnabled(true);
        stopButton.setEnabled(false);
    }

    public void testbuttonclick(View view) {
        model.newEvent(-1.0);
        updateOutput();
    }

    private void updateOutput() {
        ArrayList<DetectedEvent> events = model.getEvents();

        String s = "";
        for (DetectedEvent event : events) {
            s += " \n" + event.getTimeStamp() + " " + event.getStrength();
        }

        outputText.setText(s);
    }

    // The thread to run the timer
    // UI changes must be posted back to the main thread!
    class Timer implements Runnable {
        @Override
        public void run() {
            Handler mainThreadHandler = new Handler(Looper.getMainLooper());

            long currentTime;
            long displayedInterval = 0;

            while (model.isTimerRunning()) {
                currentTime = System.currentTimeMillis();
                long currentInterval = (currentTime - model.getStartTime()) / 1000;

                // Ticker
                if (currentInterval >= displayedInterval + 1) {
                    displayedInterval = currentInterval;

                    int minutes = (int) displayedInterval / 60;
                    int seconds = (int) displayedInterval % 60;
                    String s = minutes + " m " + seconds + " s";

                    mainThreadHandler.post(new Runnable() {
                        @Override
                        public void run() {
                            txtTime.setText(s);
                        }
                    });
                }

                /*
                // New event every minute for testing
                if (currentInterval % 60 == 0) {
                    model.newEvent(-9.9);
                    mainThreadHandler.post(() -> updateOutput());
                }
                 */

                // Stop timer & recording when it reaches MAX_TIME
                if (currentInterval > MAX_TIME) {
                    mainThreadHandler.post(() -> stopRecording());
                }

                try {
                    Thread.sleep(500);
                } catch (InterruptedException e) {
                    e.printStackTrace();
                }
            }
        }
    }

}