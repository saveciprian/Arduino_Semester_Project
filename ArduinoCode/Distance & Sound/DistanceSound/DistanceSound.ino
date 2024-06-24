#include "pitches.h"
#define BUZZER_PIN 8

float distance, duration;
const int echo = A2; // selected pins
const int trigger = A3;

int melody[] = {
  NOTE_E3, NOTE_E2, NOTE_B3, NOTE_B2, NOTE_B1, NOTE_GS2, NOTE_E2
};

int durations[] = {
  8, 8, 16, 16, 16, 8, 4
};

bool playSound = false;
int incomingByte[2];

void setup() {
    Serial.begin(115200);
    pinMode(echo, INPUT);
    pinMode(trigger, OUTPUT);

    pinMode(BUZZER_PIN, OUTPUT);
}

void loop() {
    if (Serial.available() > 0){
      while (Serial.peek() == 'B'){
        Serial.read();
        incomingByte[0] = Serial.parseInt();
      if (incomingByte[0] == 1)
        { playSound = true; }
      }
      while (Serial.available() > 0)
      {
        Serial.read();
      }
    }

    if(playSound)
    {   
      int size = sizeof(durations) / sizeof(int);
      for (int note = 0; note < size; note++) {
        //to calculate the note duration, take one second divided by the note type.
        //e.g. quarter note = 1000 / 4, eighth note = 1000/8, etc.
        int duration = 1000 / durations[note];
        tone(BUZZER_PIN, melody[note], duration);

        //to distinguish the notes, set a minimum time between them.
        //the note's duration + 30% seems to work well:
        int pauseBetweenNotes = duration * 1.30;
        delay(pauseBetweenNotes);
        
        //stop the tone playing:
        noTone(BUZZER_PIN);
      }
      playSound = false;
    }


    // Trigger the sensor
    digitalWrite(trigger, LOW);
    delayMicroseconds(2);
    digitalWrite(trigger, HIGH);
    delayMicroseconds(10);
    digitalWrite(trigger, LOW);
    
    // Read the echo
    duration = pulseIn(echo, HIGH);
    
    // Calculate the distance
    distance = (duration * 0.0343) / 2; // Speed of sound is 343 m/s, which is 0.0343 cm/us

    // Check if the distance is within the permissible range
    if (distance < 2 || distance > 450) {
        Serial.println("Distance=0"); // Send 0 if out of range
    } else {
        Serial.print("Distance=");
        Serial.println(distance);
    }

    // Small delay to avoid sensor flooding
    delay(50); // 20 measurements per second
}
