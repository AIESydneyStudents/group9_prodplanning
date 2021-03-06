﻿/*----------------------------------------------
    File Name: SpawnerManager.cs
    Purpose: Spawn items in the game
    Author: Logan Ryan
    Modified: 24 November 2020
------------------------------------------------
    Copyright 2020 Caffeinated.
----------------------------------------------*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerManager : MonoBehaviour
{
    public GameObject[] spawners;
    public GameObject[] itemToBeSpawned;
    public Vector3 offset = new Vector3(0.4f, 0.1f, 0.3f);
    public WindowQuestPointer windowQuestPointer;
    public int numberOfItemsToBeSpawned;

    private int index = 0;
    private int emptySpawners = 0;
    private int customerOnSpawner;
    private DestroyCollectable destroyCollectable;
    private int itemsInScene;
    private List<int> usedSpawners = new List<int>();

    private bool teaSpawner;
    private bool powerUpSpawner;
    private bool customerSpawner;

    /// <summary>
    /// Start is called just before any of the Update methods is called the first time
    /// </summary>
    private void Start()
    {
        destroyCollectable = GameObject.Find("Player").GetComponent<DestroyCollectable>();

        // Tag the spawner based on what item they are spawning
        if (itemToBeSpawned[0].CompareTag("Collectable"))
        {
            teaSpawner = true;
        }
        else if (itemToBeSpawned[0].CompareTag("PowerUp"))
        {
            powerUpSpawner = true;
        }
        else if (itemToBeSpawned[0].CompareTag("Customer"))
        {
            customerSpawner = true;
        }
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled
    /// </summary>
    void Update()
    {
        if (teaSpawner)
        {
            SpawnTea();
        }
        else if (powerUpSpawner)
        {
            SpawnPowerUp();
        }
        else if (customerSpawner)
        {
            SpawnCustomer();
        }

        // Set the target object for the window quest pointer
        if (windowQuestPointer != null)
        {
            if (GameObject.Find("TeaBag(Clone)"))
            {
                windowQuestPointer.Show(GameObject.FindGameObjectWithTag("Collectable"));
            }
            else if (GameObject.FindGameObjectWithTag("Customer"))
            {
                windowQuestPointer.Show(GameObject.FindGameObjectWithTag("Customer"));
            }
        }
    }

    /// <summary>
    /// Spawn tea bags
    /// </summary>
    private void SpawnTea()
    {
        for (int i = 0; i < spawners.Length; i++)
        {
            if (GameObject.Find(itemToBeSpawned[0].name + "(Clone)") || destroyCollectable.teaBags > 0)
            {
                break;
            }
            else
            {
                emptySpawners++;
            }
        }

        if (emptySpawners == spawners.Length)
        {
            index = Random.Range(0, spawners.Length);
            Instantiate(itemToBeSpawned[0], spawners[index].transform.position + offset, Quaternion.identity);
            emptySpawners = 0;
        }
    }

    /// <summary>
    /// Spawn power ups
    /// </summary>
    private void SpawnPowerUp()
    {
        // Spawn the items based on how many items that need to be spawned
        List<int> availableSpawners = new List<int>();
        itemsInScene = 0;

        // Check if the number of items has been spawned
        for (int i = 0; i < spawners.Length; i++)
        {
            if (GameObject.Find(itemToBeSpawned[0].name + i))
            {
                itemsInScene++;
            }
        }

        if (itemsInScene != numberOfItemsToBeSpawned)
        {
            // Add the spawners that are available
            for (int i = 0; i < spawners.Length; i++)
            {
                availableSpawners.Add(i);
            }

            for (int l = 0; l < usedSpawners.Count; l++)
            {
                GameObject powerUp = GameObject.Find(itemToBeSpawned[0].name + l);

                if (powerUp == null)
                {
                    // Check if the spawner already exists in the available spawner list
                    availableSpawners.Add(l);
                    usedSpawners.Remove(l);
                }
            }

            // Remove the spawners that are being used
            for (int j = 0; j < availableSpawners.Count; j++)
            {
                for (int k = 0; k < usedSpawners.Count; k++)
                {
                    if (availableSpawners[j] == usedSpawners[k])
                    {
                        availableSpawners.RemoveAt(j);
                    }
                }
            }

            // If not, then choose a random available spawner
            while (itemsInScene < numberOfItemsToBeSpawned)
            {
                int availableSpawnersIndex = Random.Range(0, availableSpawners.Count);
                index = availableSpawners[availableSpawnersIndex];
                GameObject powerUp = Instantiate(itemToBeSpawned[0], spawners[index].transform.position + offset, Quaternion.identity);
                // Naming convention: PU_Speed0
                powerUp.name = itemToBeSpawned[0].name + index;
                availableSpawners.RemoveAt(availableSpawnersIndex);
                itemsInScene++;
                usedSpawners.Add(index);
            }
        }
    }

    /// <summary>
    /// Spawn Customers
    /// </summary>
    private void SpawnCustomer()
    {
        for (int i = 0; i < spawners.Length; i++)
        {
            if (GameObject.FindGameObjectWithTag("Customer"))
            {
                break;
            }
            else
            {
                emptySpawners++;
            }
        }

        if (emptySpawners == spawners.Length)
        {
            index = Random.Range(0, spawners.Length);
            int customerIndex = Random.Range(0, itemToBeSpawned.Length);
            Instantiate(itemToBeSpawned[customerIndex], spawners[index].transform.position + offset, Quaternion.Euler(0, 180, 0));
            emptySpawners = 0;
        }
    }
}
