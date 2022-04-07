package com.example.spasmometer;

import android.content.Context;

import androidx.lifecycle.ViewModel;
import java.io.BufferedWriter;
import java.io.File;
import java.io.FileWriter;
import java.io.IOException;
import java.text.DecimalFormat;
import java.util.ArrayList;

public class SpasmometerModel extends ViewModel {
    private long startTime;
    private long stopTime;
    private boolean isTimerRunning;

    private ArrayList<DetectedEvent> eventList;

    public void startTimer() {
        eventList = new ArrayList<DetectedEvent>();
        startTime = System.currentTimeMillis();
        isTimerRunning = true;
    }

    public long stopTimer() {
        stopTime = System.currentTimeMillis();
        isTimerRunning = false;
        return (this.stopTime - this.startTime) / 1000;
    }

    public void newEvent(double strength) {
        if (isTimerRunning) {
            float t = (float) (System.currentTimeMillis() - startTime) / 1000;
            DetectedEvent event = new DetectedEvent(t, (float) strength);
            eventList.add(event);
        }
    }

    public boolean saveRecording(Context c) {
        // Formatter for numbers
        DecimalFormat formatter = new DecimalFormat("##0.0");

        long timeStampMs = System.currentTimeMillis();
        int timeStampMin = (int) (timeStampMs / 60000);
        String timeString = Integer.toString(timeStampMin);

        File saveFile = new File(c.getFilesDir(), "output" + timeString + ".csv");

        try (BufferedWriter writer = new BufferedWriter(new FileWriter(saveFile))) {
            writer.write("Time,Strength\n");

            for (DetectedEvent event : eventList) {
                writer.write(formatter.format(event.getTimeStamp()) + ",");
                writer.write(formatter.format(event.getStrength()) + "\n");
            }

        } catch (IOException e) {
            e.printStackTrace();
            return false;
        }

        return true;
    }

    public ArrayList<DetectedEvent> getEvents() {
        return eventList;
    }

    public long getStartTime() {
        return startTime;
    }

    public boolean isTimerRunning() {
        return isTimerRunning;
    }
}