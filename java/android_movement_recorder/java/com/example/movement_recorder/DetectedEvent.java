package com.example.spasmometer;

public class DetectedEvent {
    private final float timeStamp;
    private final float strength;

    public DetectedEvent(float t, float s) {
        this.timeStamp = t;
        this.strength = s;
    }

    public float getTimeStamp() {
        return timeStamp;
    }

    public float getStrength() {
        return strength;
    }
}
