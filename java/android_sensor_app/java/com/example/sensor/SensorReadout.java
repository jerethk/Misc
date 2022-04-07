package com.example.sensor;

import androidx.appcompat.app.AppCompatActivity;

import android.content.Context;
import android.content.Intent;
import android.hardware.Sensor;
import android.hardware.SensorEvent;
import android.hardware.SensorEventListener;
import android.hardware.SensorManager;
import android.os.Bundle;
import android.widget.TextView;

import java.math.BigDecimal;
import java.math.MathContext;

public class SensorReadout extends AppCompatActivity {
    private SensorManager sensorManager;
    private Sensor accelSensor;

    private TextView tvTitle;
    private TextView tvX;
    private TextView tvY;
    private TextView tvZ;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_sensor_readout);

        Intent intent = getIntent();
        int sensorType = intent.getIntExtra("type", Sensor.TYPE_GRAVITY);

        tvTitle = findViewById(R.id.tvTitle);
        tvX = findViewById(R.id.tvX);
        tvY = findViewById(R.id.tvY);
        tvZ = findViewById(R.id.tvZ);

        String title = "Title";
        float conversionFactor = 1.0f;
        int rate = SensorManager.SENSOR_DELAY_NORMAL;
        float minValue = 0;

        switch (sensorType) {
            case Sensor.TYPE_GRAVITY:
                title = "Gravity";
                minValue = 0.05f;
                break;

            case Sensor.TYPE_LINEAR_ACCELERATION:
                title = "Acceleration";
                minValue = 3.0f;
                break;

            case Sensor.TYPE_GYROSCOPE:
                title = "Gyroscope";
                conversionFactor = (float) (180 / 3.14);    // convert from radians to degrees
                minValue = 0.785f;
                break;

            case Sensor.TYPE_ROTATION_VECTOR:
                title = "Rotation";
                conversionFactor = 100.0f;  // multiply by 100
                break;

            case Sensor.TYPE_MAGNETIC_FIELD:
                title = "Magnetic field";
                break;

            case Sensor.TYPE_ORIENTATION:
                title = "Orientation";
                break;
        }

        tvTitle.setText(title);

        // Sensor
        sensorManager = (SensorManager) getSystemService(Context.SENSOR_SERVICE);
        accelSensor = sensorManager.getDefaultSensor(sensorType);

        float finalCFactor = conversionFactor;
        float finalMinValue = minValue;
        sensorManager.registerListener(new SensorEventListener() {
            @Override
            public void onSensorChanged(SensorEvent e) {
                MathContext mc = new MathContext(3);

                float x = e.values[0];
                float y = e.values[1];
                float z = e.values[2];
                BigDecimal xd;
                BigDecimal yd;
                BigDecimal zd;

                if (Math.abs(x) > finalMinValue)
                    xd = new BigDecimal(x * finalCFactor).round(mc);
                else
                    xd = new BigDecimal(0);

                if (Math.abs(y) > finalMinValue)
                    yd = new BigDecimal(y * finalCFactor).round(mc);
                else
                    yd = new BigDecimal(0);

                if (Math.abs(z) > finalMinValue)
                    zd = new BigDecimal(z * finalCFactor).round(mc);
                else
                    zd = new BigDecimal(0);

                tvX.setText(xd.toString());
                tvY.setText(yd.toString());
                tvZ.setText(zd.toString());
            }

            @Override
            public void onAccuracyChanged(Sensor sensor, int i) {

            }
        }, accelSensor, rate);
    }
}