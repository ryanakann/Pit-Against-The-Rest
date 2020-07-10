using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreedingManager : MonoBehaviour
{
    public static Animal Breed(Animal animal1, Animal animal2) {
        Animal animal3 = Animal.RandomAnimal();
        return animal3;
    }
}
