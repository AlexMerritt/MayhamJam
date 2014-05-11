using UnityEngine;

/// <summary>
/// Creating instance of particles from code with no effort
/// </summary>
public class ParticleHelper : MonoBehaviour
{
	/// <summary>
	/// Singleton
	/// </summary>
	public static ParticleHelper Instance;
	
	public ParticleSystem bloodEffect;
	
	void Awake()
	{
		// Register the singleton
		if (Instance != null)
		{
			Debug.LogError("Multiple instances of ParticleHelper!");
		}
		
		Instance = this;
	}
	
	public void Splat(Vector3 position)
	{
		// Smoke on the water
		instantiate(bloodEffect, position);
	}
	
	/// <summary>
	/// Instantiate a Particle system from prefab
	/// </summary>
	/// <param name="prefab"></param>
	/// <returns></returns>
	private ParticleSystem instantiate(ParticleSystem prefab, Vector3 position)
	{
		ParticleSystem newParticleSystem = Instantiate(
			prefab,
			position,
			Quaternion.identity
			) as ParticleSystem;
			
		newParticleSystem.transform.rotation = Quaternion.Euler(-45, 0, 0);
		
		// Make sure it will be destroyed
		Destroy(
			newParticleSystem.gameObject,
			newParticleSystem.startLifetime
			);
		
		return newParticleSystem;
	}
}