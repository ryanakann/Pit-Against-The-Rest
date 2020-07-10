using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : MonoBehaviour
{
    public AnimalEnum AnimalType {get; private set;}

    #region STATS
    [SerializeField] [Range(0f, 100f)] float stamina;
    [SerializeField] [Range(0f, 100f)] float navigation;
    [SerializeField] [Range(0f, 100f)] float running;
    [SerializeField] [Range(0f, 100f)] float swimming;
    [SerializeField] [Range(0f, 100f)] float climbing;
    [SerializeField] [Range(0f, 100f)] float style;
    #endregion

    public static Animal RandomAnimal () {
        Animal animal = new Animal();
        System.Array values = System.Enum.GetValues(typeof(AnimalEnum));
        AnimalEnum animalType = (AnimalEnum)values.GetValue(Random.Range(0, values.Length));
        animal.AnimalType = animalType;

        animal.stamina = Random.Range(0f, 100f);
        animal.navigation = Random.Range(0f, 100f);
        animal.running = Random.Range(0f, 100f);
        animal.swimming = Random.Range(0f, 100f);
        animal.climbing = Random.Range(0f, 100f);
        animal.style = Random.Range(0f, 100f);
        return animal;
    }
}
