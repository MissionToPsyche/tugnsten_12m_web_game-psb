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

    public (Dictionary<string, Element>, Dictionary<string, Element>) GetData()
    {
        List<Element> allElements = EmissionSpectra.elements.Values.ToList();
        Dictionary<string, Element> trueSelected = new();
        Dictionary<string, Element> falseSelected = new();

        // Randomly gets unique elements 
        for (int i = 0; i < numTrueElements + numFalseElements; i++)
        {
            // Selects a random index
            int selectedIndex = Random.Range(0, allElements.Count);

            // Adds the first numTrueElements to trueSelected, then
            // numFalseElements to falseElements.
            if (i < numTrueElements)
            {
                // Quantity randomization is only needed for true elements
                allElements[selectedIndex].quantity = Random.Range(minQuantity, 1.0f);
                trueSelected.Add(allElements[selectedIndex].name, allElements[selectedIndex]);
            }
            else
            {
                falseSelected.Add(allElements[selectedIndex].name, allElements[selectedIndex]);
            }

            // Removes the selected element so it can't be selected twice
            allElements.RemoveAt(selectedIndex);
        }

        return (trueSelected, falseSelected);
    }
}
