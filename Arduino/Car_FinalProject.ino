#include <ESP8266WiFi.h>
#include <FirebaseArduino.h>


#define FIREBASE_HOST "iotclass-18t2.firebaseio.com" //Your Firebase Project URL goes here without "http:" , "\" and "/"
#define FIREBASE_AUTH "HereItGoesYourFirebaseSecret" //Your Firebase Database Secret goes here
#define WIFI_SSID "luiswifi" // "uCom-edge" //your WiFi SSID for which yout NodeMCU connects
#define WIFI_PASSWORD "unodostres" // "zsexdrcft"//Password of your wifi network 


// used for the music
const int c = 261;
const int d = 294;
const int e = 329;
const int f = 349;
const int g = 391;
const int gS = 415;
const int a = 440;
const int aS = 455;
const int b = 466;
const int cH = 523;
const int cSH = 554;
const int dH = 587;
const int dSH = 622;
const int eH = 659;
const int fH = 698;
const int fSH = 740;
const int gH = 784;
const int gSH = 830;
const int aH = 880;

// pin definition
const int d0 = 16;
const int d1 = 5;
const int d2 = 4;
const int d3 = 0;
const int d4 = 2;
const int d6 = 12;
const int d7 = 13;

// still pin definition
const int buzzerPin = d4;
const int ledPin1 = d6;
const int ledPin2 = d7;
 
int counter = 0;


// This function reads Firebase variables: foward, backward, left, right, horn.
// Once it founds a integer positive on any of the previous variables it process the task
// and decrements the value to 0. 
void CarWorkload()
{
  
  int tForward = Firebase.getInt("forward");
  int tBackward = Firebase.getInt("backward");
  bool doLeft = Firebase.getBool("left");
  bool doRight = Firebase.getBool("right");
  int doHorn = Firebase.getInt("horn");
  bool turnLights = Firebase.getBool("light");

  if(turnLights)
  {
    digitalWrite(ledPin1, HIGH);
    digitalWrite(ledPin2, HIGH);
  }else{
    digitalWrite(ledPin1, LOW);
    digitalWrite(ledPin2, LOW);
  }
  
  if(doHorn == 1)
  {   
    beep(eH, 500);
    beep(eH, 500); 
    Firebase.set("horn",0);
  }

  if(doHorn == 2)
  {    
    firstSection();
    Firebase.set("horn",0);
  }
  
  if(tForward > 0)
  {
    digitalWrite(d1, HIGH);
    
    if(doLeft)
      digitalWrite(d2, HIGH);      
    if(doRight)
      digitalWrite(d3, HIGH);
      
    delay(1000 * tForward);
    Firebase.set("forward",0);
    Firebase.set("left",false);
    Firebase.set("right",false);
  }

  if(tBackward > 0)
  {
    digitalWrite(d0, HIGH);
    
    if(doLeft)
      digitalWrite(d2, HIGH);      
    if(doRight)
      digitalWrite(d3, HIGH);
      
    delay(1000 * tBackward);
    Firebase.set("backward",0);
    Firebase.set("left",false);
    Firebase.set("right",false);
  }
  
  digitalWrite(d0, LOW);
  digitalWrite(d1, LOW);
  digitalWrite(d2, LOW);
  digitalWrite(d3, LOW);
  
  tForward = 0;
  tBackward= 0;
  doLeft = false;
  doRight = false;
  doHorn = 0;
}

void resetFirebaseVariables()
{
  Firebase.set("forward",0);
  Firebase.set("backward",0);
  Firebase.set("left",false);
  Firebase.set("right",false);
  Firebase.set("horn",0);
}

void setup() {
  // put your setup code here, to run once:
  Serial.begin(115200); //baud rate ,if you want to see the process in the serial monitor ,same baud rate should be set.
  pinMode(d0, OUTPUT);
  pinMode(d1, OUTPUT);
  pinMode(d2, OUTPUT);
  pinMode(d3, OUTPUT);
  
  pinMode(buzzerPin, OUTPUT); //d4
  pinMode(ledPin1, OUTPUT);   //d6
  pinMode(ledPin2, OUTPUT);   //d7
  
  WiFi.begin(WIFI_SSID,WIFI_PASSWORD);
  Serial.print("connecting");
  while (WiFi.status()!=WL_CONNECTED){
    Serial.print(".");
    delay(500);
  }
  
  Serial.println();
  Serial.print("connected:");
  Serial.println(WiFi.localIP());

  // Set variables to default values
  Firebase.begin(FIREBASE_HOST,FIREBASE_AUTH);
  resetFirebaseVariables();
  Firebase.set("light",false);
}

void firebasereconnect()
{
  Serial.println("Trying to reconnect");
    Firebase.begin(FIREBASE_HOST, FIREBASE_AUTH);
  }
  
void loop() {
  if (Firebase.failed()) {
      Serial.print("setting number failed:");
      Serial.println(Firebase.error());
      firebasereconnect();
      delay(1000); // 1 full second delay
      return;
  }
  CarWorkload();
  delay(1000); // 1 full second delay
}

// song logic

void beep(int note, int duration)
{
  //Play tone on buzzerPin
  tone(buzzerPin, note, duration);
 
  //Play different LED depending on value of 'counter'
  if(counter % 2 == 0)
  {
    digitalWrite(ledPin1, HIGH);
    delay(duration);
    digitalWrite(ledPin1, LOW);
  }else
  {
    digitalWrite(ledPin2, HIGH);
    delay(duration);
    digitalWrite(ledPin2, LOW);
  }
 
  //Stop tone on buzzerPin
  noTone(buzzerPin);
 
  delay(50);
 
  //Increment counter
  counter++;
}
 
void firstSection()
{
  beep(a, 500);
  beep(a, 500);    
  beep(a, 500);
  beep(f, 350);
  beep(cH, 150);  
  beep(a, 500);
  beep(f, 350);
  beep(cH, 150);
  beep(a, 650);
 
  delay(500);
 
  beep(eH, 500);
  beep(eH, 500);
  beep(eH, 500);  
  beep(fH, 350);
  beep(cH, 150);
  beep(gS, 500);
  beep(f, 350);
  beep(cH, 150);
  beep(a, 650);
 
  delay(500);
}

