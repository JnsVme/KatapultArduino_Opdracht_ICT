#include <Servo.h>
#include <Stepper.h>

#define STEPS_PER_REV 2048 
#define MOTOR_SPEED 10     

Servo servo1;  
Servo servo2;  


Stepper stepper(STEPS_PER_REV, 8, 10, 9, 11);




void setup() {
    Serial.begin(9600);

   
    servo1.attach(3);
    servo2.attach(5);

   

    // Set stepper motor speed
    stepper.setSpeed(MOTOR_SPEED);
    stepper.step(1); //voor 1 of ander reden als ik dit toevoeg in de code kan hij wel naar links of rechts.

    Serial.println("Ready!");
}

void loop() {
    if (Serial.available()) {
        String input = Serial.readStringUntil('\n');  
        input.trim();  

        if (input.length() > 0) {
            
            if (input == "A") {  
                delay(2000);
                servo1.write(180);
                servo2.write(90);
                stepper.step(-13600);
                delay(500);
                servo1.write(0);
                stepper.step(13600);
            } 
            else if (input == "B") {
                
                servo1.write(180);
            }
            
            else if (input.startsWith("M")) {  
                
                String angleStr = input.substring(1); 
                int angle = angleStr.toInt(); 

              
                if (angle >= 0 && angle <= 180) {
                    servo2.write(angle);  
                    Serial.print("Servo 2 moved to: ");
                    Serial.println(angle);
                } else {
                    Serial.println("Invalid angle. Please enter a value between 45 and 145.");
                }
            }
          
        }
    }
}
