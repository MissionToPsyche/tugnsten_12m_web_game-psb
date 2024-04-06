using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;
using System.IO;
using UnityEngine.EventSystems;

public class sliceImageTests
{
    // A Test behaves as an ordinary method
    // [Test]
    // public void sliceImageTestsSimplePasses()
    // {
    //     // Use the Assert class to test conditions
    // }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator numImages()
    {
        SliceImage sliceImage = new GameObject().AddComponent<SliceImage>();
        
        // Set up or mock the originalImage
        Texture2D originalImage = new Texture2D(512, 512);
        sliceImage.setOriginalImage(originalImage);

        // Set up or mock the Canvas
        sliceImage.setCanvas(new GameObject().AddComponent<Canvas>());

        // Execute the slice method
        sliceImage.slice();

        // assert 6 images are created
        Assert.AreEqual(sliceImage.getImages().Count, 6);

        yield return null;
    }

    [UnityTest]
    public IEnumerator differentStarts()
    {
        SliceImage sliceImage = new GameObject().AddComponent<SliceImage>();

        // Set up or mock the originalImage
        Texture2D originalImage = new Texture2D(512, 512);
        sliceImage.setOriginalImage(originalImage);

        // Set up or mock the Canvas
        sliceImage.setCanvas(new GameObject().AddComponent<Canvas>());

        // Manually set up the starts list
        sliceImage.setStarts(new List<Vector2> { new Vector2(0, 0), new Vector2(100, 100), new Vector2(200, 200) });

        // Execute the isStartDifferent method
        bool result = sliceImage.isStartDifferent(new Vector2(50, 50), new Vector2(20, 20));

        // Assert that the start is different
        Assert.IsTrue(result);

        // Execute the isStartDifferent method
        bool result2 = sliceImage.isStartDifferent(new Vector2(0, 0), new Vector2(20, 20));

        // Assert that the start is not different
        Assert.IsFalse(result2);

        yield return null;
    }

    [UnityTest]
    public IEnumerator checkOverlap()
    {
        SliceImage sliceImage = new GameObject().AddComponent<SliceImage>();

        // Set up or mock the originalImage
        Texture2D originalImage = new Texture2D(512, 512);
        sliceImage.setOriginalImage(originalImage);

        // Set up or mock the Canvas
        sliceImage.setCanvas(new GameObject().AddComponent<Canvas>());

        // Manually set up the starts list
        sliceImage.setStarts(new List<Vector2> { new Vector2(0, 0), new Vector2(100, 100), new Vector2(200, 200) });

        // Execute the isOverlap method
        bool result = sliceImage.isOverlap(new Vector2(50, 50), new Vector2(20, 20));

        // Assert that there is not an overlap
        Assert.IsFalse(result);

        // Execute the isOverlap method
        bool result2 = sliceImage.isOverlap(new Vector2(10, 10), new Vector2(20, 20));

        // Assert that there is an overlap
        Assert.IsTrue(result2);

        yield return null;
    }

    [UnityTest]
    public IEnumerator createSection()
    {
        SliceImage sliceImage = new GameObject().AddComponent<SliceImage>();

        // Set up or mock the originalImage
        Texture2D originalImage = new Texture2D(512, 512);
        sliceImage.setOriginalImage(originalImage);

        // Execute the createSectionOfOriginal method
        Texture2D slicedTexture = sliceImage.createSectionOfOriginal();

        // Assert that the slicedTexture is not null
        Assert.NotNull(slicedTexture);

        // Assert that the width and height of the slicedTexture are greater than 0
        Assert.Greater(slicedTexture.width, 0);
        Assert.Greater(slicedTexture.height, 0);

        yield return null;
    }

    [UnityTest]
    public IEnumerator createValidGameObject()
    {
        SliceImage sliceImage = new GameObject().AddComponent<SliceImage>();

        // Set up or mock the required components and variables
        sliceImage.setCanvas(new GameObject().AddComponent<Canvas>());

        float imgWidth = 50f;
        float imgHeight = 50f;
        Vector2 start = Vector2.zero;
        Texture2D slicedTexture = new Texture2D(64, 64);
        int imgNum = 0;

        // Execute the createImageObject method
        GameObject imgObject = sliceImage.createImageObject(imgWidth, imgHeight, start, slicedTexture, imgNum);

        // Assert that the imgObject is not null
        Assert.NotNull(imgObject);

        // Assert that the imgObject has the expected name
        Assert.AreEqual("img0", imgObject.name);

        // Assert that the RectTransform component is attached
        RectTransform trans = imgObject.GetComponent<RectTransform>();
        Assert.NotNull(trans);

        // Assert that the CanvasGroup component is attached
        CanvasGroup group = imgObject.GetComponent<CanvasGroup>();
        Assert.NotNull(group);
        Assert.AreEqual(1f, group.alpha);
        Assert.IsTrue(group.blocksRaycasts);

        // Assert that the Image component is attached
        Image image = imgObject.GetComponent<Image>();
        Assert.NotNull(image);

        yield return null;
    }

    // SnapIfInRange snaps RectTransform to nearest snap point within snapRadius
    [Test]
    public void test_snap_if_in_range_snaps_rect_transform()
    {
        // Arrange
        GameObject gameObject = new GameObject("img1");
        SnapToTarget snapToTarget = gameObject.AddComponent<SnapToTarget>();
        RectTransform rectTransform = gameObject.AddComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(100f, 100f);
        ImageController imageController = gameObject.AddComponent<ImageController>();
        GameObject gameObject2 = new GameObject("img2");
        RectTransform rectTransform2 = gameObject2.AddComponent<RectTransform>();
        rectTransform2.anchoredPosition = new Vector2(125f, 125f);
        Dictionary<string, Vector2> offsets = new Dictionary<string, Vector2>();
        offsets.Add("img1", new Vector2(-10f, -10f));
        offsets.Add("img2", new Vector2(10f, 10f));
        imageController.setSnapOffsets(offsets);
        List<GameObject> snapPoints = new List<GameObject>();
        snapPoints.Add(gameObject);
        snapPoints.Add(gameObject2);
        imageController.setSnapPoints(snapPoints);

        // Act
        snapToTarget.SnapIfInRange();

        // Assert
        Assert.AreEqual(new Vector2(135, 135), snapToTarget.GetComponent<RectTransform>().anchoredPosition);
    }

    // Draggable image can be dragged within defined boundaries
    [UnityTest]
    public IEnumerator DraggableImageCanBeDraggedWithinBoundaries()
    {
        // Arrange
        GameObject draggableObject = new GameObject();
        draggableObject.SetActive(false);
        Draggable draggable = draggableObject.AddComponent<Draggable>();
        RectTransform rectTransform = draggableObject.AddComponent<RectTransform>();
        CanvasGroup canvasGroup = draggableObject.AddComponent<CanvasGroup>();
        // ImagerGameController imagerGameController = new GameObject().AddComponent<ImagerGameController>();
        SnapToTarget snapToTarget = draggableObject.AddComponent<SnapToTarget>();
        draggableObject.SetActive(true);

        // draggable.rectTransform = rectTransform;
        // draggable.canvasGroup = canvasGroup;
        // draggable.imagerGameController = imagerGameController;
        // draggable.snapToTarget = snapToTarget;

        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = new Vector2(1, 1);

        // Act
        draggable.OnBeginDrag(eventData);
        draggable.OnDrag(eventData);
        Vector3 newPosition = rectTransform.position;

        // Assert
        Assert.AreEqual(newPosition.x, 1);
        Assert.AreEqual(newPosition.y, 1);

        yield return null;
    }
}
