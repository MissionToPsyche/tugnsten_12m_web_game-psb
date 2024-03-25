using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpectDataGenerator : MonoBehaviour
{
    // Elements used to make the reference graph
    public const int numTrueElements = 3;
    // Extra elements given to the user that are not in the reference graph
    public const int numFalseElements = 1;

    public const float minQuantity = 0.1f;

    public (SortedDictionary<string, Element>, SortedDictionary<string, Element>) GetData()
    {
        // Deep copies the element dictionary
        SortedDictionary<string, Element> allElements = new(EmissionSpectra.elements.ToDictionary(p => p.Key, p => p.Value.Clone()));
        
        // Converts the dictionary keys to a list for ease of use
        List<Element> allElementsList = allElements.Values.ToList();
        
        SortedDictionary<string, Element> trueSelected = new();
        SortedDictionary<string, Element> falseSelected = new();

        // Randomly gets unique elements 
        for (int i = 0; i < numTrueElements + numFalseElements; i++)
        {
            // Selects a random index
            int selectedIndex = Random.Range(0, allElementsList.Count);

            // Adds the first numTrueElements to trueSelected, then
            // numFalseElements to falseElements.
            if (i < numTrueElements)
            {
                // Quantity randomization is only needed for true elements
                allElementsList[selectedIndex].quantity = Random.Range(minQuantity, 1.0f);
                trueSelected.Add(allElementsList[selectedIndex].name, allElementsList[selectedIndex]);
            }
            else
            {
                falseSelected.Add(allElementsList[selectedIndex].name, allElementsList[selectedIndex]);
            }

            // Removes the selected element so it can't be selected twice
            allElementsList.RemoveAt(selectedIndex);
        }

        return (trueSelected, falseSelected);
    }
}
