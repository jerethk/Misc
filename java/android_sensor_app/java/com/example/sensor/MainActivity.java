package com.example.sensor;

import androidx.appcompat.app.AppCompatActivity;

import android.content.Intent;
import android.hardware.Sensor;
import android.os.Bundle;
import android.view.View;
import android.widget.RadioButton;

public class MainActivity extends AppCompatActivity {
    private RadioButton rbGrav;
    private RadioButton rbAccel;
    private RadioButton rbRotation;
    private RadioButton rbOrient;
    private RadioButton rbGyro;
    private RadioButton rbMag;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        rbGrav = findViewById(R.id.rbGravity);
        rbRotation = findViewById(R.id.rbRotation);
        rbAccel = findViewById(R.id.rbAccel);
        rbGyro = findViewById(R.id.rbGyro);
        rbMag = findViewById(R.id.rbMag);
        rbOrient = findViewById(R.id.rbOrient);
    }

    public void btnGoClick(View view) {
        int selection = 0;

        if (rbGrav.isChecked()) selection = Sensor.TYPE_GRAVITY;
        if (rbRotation.isChecked()) selection = Sensor.TYPE_ROTATION_VECTOR;
        if (rbAccel.isChecked()) selection = Sensor.TYPE_LINEAR_ACCELERATION;
        if (rbGyro.isChecked()) selection = Sensor.TYPE_GYROSCOPE;
        if (rbMag.isChecked()) selection = Sensor.TYPE_MAGNETIC_FIELD;
        if (rbOrient.isChecked()) selection = Sensor.TYPE_ORIENTATION;

        Intent intent = new Intent(this, SensorReadout.class);
        intent.putExtra("type", selection);
        //intent.putExtra("factor", factor);
        startActivity(intent);
    }

}