using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicWeather : MonoBehaviour
{
	private Transform _player;                                                      //player gameobject transform
	public Transform _weather;                                                     //weather gameobject transform
	public float _weatherheight = 15f;                                              //defines height from ground of weather game object

	public ParticleSystem _sunCloudsParticleSystem;                                //creates slot in inspector to assign our sun cloud system
	public ParticleSystem _thunderStormParticleSystem;                                //creates slot in inspector to assign our thunder storm system
	public ParticleSystem _mistParticleSystem;                                //creates slot in inspector to assign our mist system
	public ParticleSystem _snowParticleSystem;                                //creates slot in inspector to assign our snow system

	private ParticleSystem.EmissionModule _sunClouds;                              //defines naming convention for sun clouds emission module
	private ParticleSystem.EmissionModule _thunder;                              //defines naming convention for thunder emission module
	private ParticleSystem.EmissionModule _mist;                              //defines naming convention for mist emission module
	private ParticleSystem.EmissionModule _snow;                              //defines naming convention for snow emission module

	public float _switchWeatherTimer = 0f;                                          //switch weather timer equals 0

	public float _resetWeatherTimer = 10f;                                          //defined value to reset weather timer too
	
	public float _lightDimTime = 0.1f;                                              //defines our rate for light diming
	public float _thunderIntensity = 0f;                                            //defines our thunder light intensity
	public float _sunnyIntensity = 1f;                                            //defines our sunny light intensity
	public float _mistIntensity = 0.5f;                                             //defines our mist light intensity
	public float _snowIntensity = 0.75f;                                            //defines our snow light intensity

	public Color _sunFog;                                                           //create slot in the inspector to set sun fog color
	public Color _thunderFog;                                                           //create slot in the inspector to set thunder fog color
	public Color _mistFog;                                                           //create slot in the inspector to set sun mist color
	public Color _snowFog;                                                           //create slot in the inspector to set snow fog color
	public float _fogChangeSpeed = 0.1f;                                            //defines speed in which the fog will change

	public WeatherStates _weatherstate;                                             // defined the naming convention of our weather states

	private int _switchWeather;                                                     // defines naming convention of our switch range

	public enum WeatherStates
	{                                                       // defined all states the weather can exists
		PickWeather,
		SunnyWeather,
		ThunderWeather,
		MistWeather,
		SnowWeather
	}

	// Start is called before the first frame update
	void Start()
	{
		GameObject _playerGameObject = GameObject.FindGameObjectWithTag("Player");      //find player game object
		_player = _playerGameObject.transform;                                          //caches players position

		GameObject _weatherGameObject = GameObject.FindGameObjectWithTag("Weather");      //find weather game object
		_weather = _weatherGameObject.transform;                                          //caches weathers position

		RenderSettings.fog = true;                                                      //enable fog in render settings
		RenderSettings.fogMode = FogMode.ExponentialSquared;                            //set fog mode to exponential squared
		RenderSettings.fogDensity = 0.02f;                                              //set fog density

		_sunClouds = _sunCloudsParticleSystem.emission;                                 //_sunClouds is equal to the sun clouds particle system
		_thunder = _thunderStormParticleSystem.emission;                                   //_thunder is equal to the thunder particle system
		_mist = _mistParticleSystem.emission;                                      //_mist is equal to the mist particle system
		_snow = _snowParticleSystem.emission;                                      //_snow is equal to the snow particle system

		StartCoroutine(WeatherFSM());                                               //start the finite state machine
	}

	// Update is called once per frame
	void Update()
	{
		SwitchWeatherTimer();                                                           //updates our SwitchWeatherTimer function
		_weather.transform.rotation = _player.transform.rotation;
		_weather.transform.position = new Vector3(_player.position.x , _player.position.y + _weatherheight, _player.position.z);	

	}

	void SwitchWeatherTimer()
	{
		Debug.Log("SwitchWeatherTimer");

		_switchWeatherTimer -= Time.deltaTime;                                          // decrease weather timer by real time

		if (_switchWeatherTimer < 0)                                                    //if switch timer is greater than zero 
			_switchWeatherTimer = 0;                                                    //then switch timer is equal to zero

		if (_switchWeatherTimer > 0)                                                    //if switch timer is greater than zero 
			return;                                                                     //then do nothing and return

		if (_switchWeatherTimer == 0)                                                   //if switch timer is equal to zero 
			_weatherstate = DynamicWeather.WeatherStates.PickWeather;                   //then switch our case block to pick weather

		_switchWeatherTimer = _resetWeatherTimer;                                   // switch timer is equal to reset timer
	}

	IEnumerator WeatherFSM()
	{
		while (true)                    //while the weather state machine is active
		{
			switch (_weatherstate)
			{    // switch the weather states
				case WeatherStates.PickWeather:
					PickWeather();
					break;
				case WeatherStates.SunnyWeather:
					SunnyWeather();
					break;
				case WeatherStates.ThunderWeather:
					ThunderWeather();
					break;
				case WeatherStates.MistWeather:
					MistWeather();
					break;
				case WeatherStates.SnowWeather:
					SnowWeather();
					break;
			}
			yield return null;
		}
	}

	void PickWeather()
	{
		Debug.Log("PickWeather");
		_switchWeather = Random.Range(0, 5);                                     //_switchWeather is equal to a random range between (0,5)

		_sunClouds.enabled = false;                         //disables our sun clouds particle system
		_thunder.enabled = false;                           //disables our thunder storm particle system
		_mist.enabled = false;                              //disables our mist particle system
		_snow.enabled = false;                               //disables our snow particle system

		switch (_switchWeather)
		{
			case 0:
				_weatherstate = DynamicWeather.WeatherStates.SunnyWeather;
				break;
			case 1:
				_weatherstate = DynamicWeather.WeatherStates.ThunderWeather;
				break;
			case 2:
				_weatherstate = DynamicWeather.WeatherStates.MistWeather;
				break;
			case 3:
				_weatherstate = DynamicWeather.WeatherStates.SnowWeather;
				break;
		}
	}

	void SunnyWeather()
	{
		Debug.Log("SunnyWeather");
		_sunClouds.enabled = true;                         //enables our sun clouds particle system

		if (GetComponent<Light>().intensity > _sunnyIntensity)              //if the light intensity is greater than sunny intensity
			GetComponent<Light>().intensity -= Time.deltaTime * _lightDimTime;  //then minus our intensity by our light dim time

		if (GetComponent<Light>().intensity < _sunnyIntensity)                //if the light intensity is less than sunny intensity
			GetComponent<Light>().intensity += Time.deltaTime * _lightDimTime;  //then add our intensity by our light dim time

		Color _currentFogColor = RenderSettings.fogColor;                           //_currentFogColor is equal to the render settings fog color

		RenderSettings.fogColor = Color.Lerp(_currentFogColor, _sunFog, _fogChangeSpeed * Time.deltaTime);          //render settings fog equals (change from current to sun fog by fog change speed)

	}

	void ThunderWeather()
	{
		Debug.Log("ThunderWeather");
		_thunder.enabled = true;                //enables our thunder storm particle system

		if (GetComponent<Light>().intensity > _thunderIntensity)                //if the light intensity is greater than thunder intensity
			GetComponent<Light>().intensity -= Time.deltaTime * _lightDimTime;  //then minus our intensity by our light dim time

		if (GetComponent<Light>().intensity < _thunderIntensity)                //if the light intensity is less than thunder intensity
			GetComponent<Light>().intensity += Time.deltaTime * _lightDimTime;  //then add our intensity by our light dim time

		Color _currentFogColor = RenderSettings.fogColor;                           //_currentFogColor is equal to the render settings fog color

		RenderSettings.fogColor = Color.Lerp(_currentFogColor, _thunderFog, _fogChangeSpeed * Time.deltaTime);          //render settings fog equals (change from current to thunder fog by fog change speed)
	}

	void MistWeather()
	{
		Debug.Log("MistWeather");
		_mist.enabled = true;                              //enables our mist particle system

		if (GetComponent<Light>().intensity > _mistIntensity)                //if the light intensity is greater than mist intensity
			GetComponent<Light>().intensity -= Time.deltaTime * _lightDimTime;  //then minus our intensity by our light dim time

		if (GetComponent<Light>().intensity < _mistIntensity)                //if the light intensity is less than mist intensity
			GetComponent<Light>().intensity += Time.deltaTime * _lightDimTime;  //then add our intensity by our light dim time

		Color _currentFogColor = RenderSettings.fogColor;                           //_currentFogColor is equal to the render settings fog color

		RenderSettings.fogColor = Color.Lerp(_currentFogColor, _mistFog, _fogChangeSpeed * Time.deltaTime);          //render settings fog equals (change from current to mist fog by fog change speed)
	}


	void SnowWeather()
	{
		Debug.Log("SnowWeather");
		_snow.enabled = true;                              //enables our snow particle system

		if (GetComponent<Light>().intensity > _snowIntensity)                //if the light intensity is greater than snow intensity
			GetComponent<Light>().intensity -= Time.deltaTime * _lightDimTime;  //then minus our intensity by our light dim time

		if (GetComponent<Light>().intensity < _snowIntensity)                //if the light intensity is less than snow intensity
			GetComponent<Light>().intensity += Time.deltaTime * _lightDimTime;  //then add our intensity by our light dim time

		Color _currentFogColor = RenderSettings.fogColor;                           //_currentFogColor is equal to the render settings fog color

		RenderSettings.fogColor = Color.Lerp(_currentFogColor, _snowFog, _fogChangeSpeed * Time.deltaTime);          //render settings fog equals (change from current to snow fog by fog change speed)
	}
}
