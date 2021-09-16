using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstaclesGenerator : MonoBehaviour
{
    public ShipController ship;
    public GameObject[] rocks;
    public GameObject textObject;
    public GameObject shipLight;
    public GameObject instructions;
    private TextMesh text;
    public float shipSpeed;
    public int startingObstaclesPerSecond;
    public int obstaclesPerSecondStep;
    public float generationTimeStep;
    public float difficultyIncreaseTimeStep;
    public float rockSize;
    public float resetTime;
    private float timeToNextObstacleGeneration;
    private float timeToDifficultyIncrease;
    private int points = 0;
    private float timeElapsed = 0;
    private float timeCrashed = 0;
    private int obstaclesPerSecond;
    private bool showInstructions = true;

    // Start is called before the first frame update
    void Start()
    {
        text = textObject.GetComponent<TextMesh>();
        timeToNextObstacleGeneration = generationTimeStep;
        timeToDifficultyIncrease = difficultyIncreaseTimeStep;
        obstaclesPerSecond = startingObstaclesPerSecond;
    }

    // Update is called once per frame
    void Update()
    {
        if(!ship.hasCrashed()){
            if(Time.time > 5 && showInstructions){
                    Destroy(instructions.gameObject);
                    showInstructions = false;
            }
            if(timeToNextObstacleGeneration <=0 ){
                if(timeToDifficultyIncrease <= 0){
                    obstaclesPerSecond += obstaclesPerSecondStep;
                    timeToDifficultyIncrease += difficultyIncreaseTimeStep;
                }
                for(int i = 0; i<obstaclesPerSecond; i++){
                    int rock_i = Random.Range(0, rocks.Length);
                    GameObject obstacle = Instantiate(rocks[rock_i], Vector3.zero, Quaternion.identity);
                    obstacle.transform.localScale = new Vector3(rockSize, rockSize, rockSize);
                    obstacle.transform.parent = transform;
                    obstacle.transform.localPosition = new Vector3(Random.Range(-20F, 20F), Random.Range(-20F, 20F), Random.Range(18F, 22F));
                    obstacle.transform.Rotate(Random.Range(-180F, 180F), Random.Range(-180F, 180F), Random.Range(-180F, 180F));
                }
                timeToNextObstacleGeneration += generationTimeStep;
            }
            Vector3 directionVersor = Camera.main.transform.forward;
            if(directionVersor.x > 0.5){
                directionVersor.x = 0.5F;
            } else if(directionVersor.x < -0.5){
                directionVersor.x = -0.5F;
            }
            if(directionVersor.y > 0.5){
                directionVersor.y = 0.5F;
            } else if(directionVersor.y < -0.5){
                directionVersor.y = -0.5F;
            }
            if(directionVersor.z < 0.5){
                directionVersor.z = 0.5F;
            }
            ship.transform.forward = directionVersor;
            ship.transform.Rotate(0, -180, -Camera.main.transform.rotation.z * 180);
            ship.transform.position = new Vector3(0,0,0);

            Vector3 direction = directionVersor * shipSpeed;
            foreach (Transform obstacle in transform)
            {
                obstacle.localPosition = obstacle.localPosition - direction;
                if(obstacle.localPosition.z < -5){
                    Destroy(obstacle.gameObject);
                }
            }
            timeToNextObstacleGeneration -= Time.deltaTime;
            timeToDifficultyIncrease -= Time.deltaTime;

            timeElapsed += Time.deltaTime;
            points = (int) (0.01 * timeElapsed * timeElapsed);
            text.text = "Points: " + points;
        } else if (timeCrashed < resetTime) {
            ship.GetComponent<Rigidbody>().velocity = Vector3.zero;
            ship.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            timeCrashed += Time.deltaTime;
            text.text = "Points: " + points + "\nGame over!\nRestarting in " + ((int) (resetTime - timeCrashed)) + " seconds";
        } else {
            restart();
        }
    }

    void LateUpdate(){
        textObject.transform.position = Camera.main.transform.position;
        textObject.transform.position += Camera.main.transform.forward * 7;
        textObject.transform.LookAt(Camera.main.transform);
        textObject.transform.Rotate(0, 180, 0);
        textObject.transform.position += Camera.main.transform.up * 3.5F;
        textObject.transform.position += Camera.main.transform.right * -5;
        shipLight.transform.position = ship.transform.position;
        shipLight.transform.position += -ship.transform.forward * 2.5F;
        shipLight.transform.LookAt(ship.transform);
        shipLight.transform.Rotate(0, 180, 0);
    }

    void restart(){
        foreach (Transform obstacle in transform){
            Destroy(obstacle.gameObject);
        }

        ship.restart();
        timeElapsed = 0;
        timeToNextObstacleGeneration = generationTimeStep;
        timeToDifficultyIncrease = difficultyIncreaseTimeStep;
        points = 0;
        timeCrashed = 0;
        obstaclesPerSecond = startingObstaclesPerSecond;
    }
}
