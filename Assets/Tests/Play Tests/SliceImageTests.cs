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
    [Test]
    public void numImages()
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
    }

    [Test]
    public void differentStarts()
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
    }

    [Test]
    public void checkOverlap()
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
    }

    [Test]
    public void createSection()
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
    }

    [Test]
    public void createValidGameObject()
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
    }

    // SnapIfInRange snaps RectTransform to nearest snap point within snapRadius
    [Test]
    public void test_snap_if_in_range_snaps_rect_transform()
    {
        // Arrange
        GameObject smObj = new GameObject();
        SoundManager sm = smObj.AddComponent<SoundManager>();
        GameObject gameObject = new GameObject("img1");
        SnapToTarget snapToTarget = gameObject.AddComponent<SnapToTarget>();
        RectTransform rectTransform = gameObject.AddComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(100f, 100f);
        ImageController imageController = gameObject.AddComponent<ImageController>();
        GameObject gameObject2 = new GameObject("img2");
        RectTransform rectTransform2 = gameObject2.AddComponent<RectTransform>();
        rectTransform2.anchoredPosition = new Vector2(125f, 125f);
        Dictionary<string, Vector2> offsets = new Dictionary<string, Vector2>();
        offsets.Add("img2", new Vector2(-10f, -10f));
        imageController.setSnapOffsets(offsets);
        List<GameObject> snapPoints = new List<GameObject>();
        snapPoints.Add(gameObject2);
        imageController.setSnapPoints(snapPoints);

        // Act
        snapToTarget.SnapIfInRange();

        // Assert
        Assert.AreEqual(new Vector2(115, 115), snapToTarget.GetComponent<RectTransform>().anchoredPosition);
    }

    // Draggable image can be dragged within defined boundaries
    // test runs fine, but does produce a null object error after due to partial mocking
    [Test]
    public void DraggableImageCanBeDraggedWithinBoundaries()
    {
        // Arrange
        GameObject draggableObject = new GameObject();
        draggableObject.SetActive(false);
        Draggable draggable = draggableObject.AddComponent<Draggable>();
        RectTransform rectTransform = draggableObject.AddComponent<RectTransform>();
        CanvasGroup canvasGroup = draggableObject.AddComponent<CanvasGroup>();
        SnapToTarget snapToTarget = draggableObject.AddComponent<SnapToTarget>();

        draggable.rectTransform = rectTransform;
        draggable.canvasGroup = canvasGroup;
        draggable.snapToTarget = snapToTarget;

        draggableObject.SetActive(true);

        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = new Vector2(1, 1);

        // Act
        draggable.OnBeginDrag(eventData);
        draggable.OnDrag(eventData);
        Vector3 newPosition = rectTransform.position;

        // Assert
        Assert.AreEqual(newPosition.x, 1);
        Assert.AreEqual(newPosition.y, 1);
    }

    // GAME CONTROLELR
    // isAllSnapPointsEqual returns true if all snap points for an image are within a tolerance
    // test runs fine, but does produce a null object error after due to partial mocking
    [Test]
    public void test_is_all_snap_points_equal()
    {
        // Arrange
        GameObject gameObject= new GameObject();
        ImagerGameController gameController = gameObject.AddComponent<ImagerGameController>();

        GameObject image1 = new GameObject("img1");
        SnapToTarget snapToTarget = image1.AddComponent<SnapToTarget>();
        RectTransform rectTransform = image1.AddComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(100f, 100f);
        ImageController imageController = image1.AddComponent<ImageController>();
        Dictionary<string, Vector2> offsets = new Dictionary<string, Vector2>();
        offsets.Add("img2", new Vector2(-10f, -10f));
        offsets.Add("img3", new Vector2(-20f, -30f));
        imageController.setSnapOffsets(offsets);
        GameObject image2 = new GameObject("img2");
        RectTransform rectTransform2 = image2.AddComponent<RectTransform>();
        rectTransform2.anchoredPosition = new Vector2(110f, 110f);
        ImageController imageController2 = image2.AddComponent<ImageController>();
        Dictionary<string, Vector2> offsets2 = new Dictionary<string, Vector2>();
        offsets2.Add("img1", new Vector2(10f, 10f));
        offsets2.Add("img3", new Vector2(10f, 20f));
        imageController2.setSnapOffsets(offsets2);
        GameObject image3 = new GameObject("img3");
        RectTransform rectTransform3 = image3.AddComponent<RectTransform>();
        rectTransform3.anchoredPosition = new Vector2(120f, 130f);
        ImageController imageController3 = image3.AddComponent<ImageController>();
        Dictionary<string, Vector2> offsets3 = new Dictionary<string, Vector2>();
        offsets3.Add("img2", new Vector2(-10f, -20f));
        offsets3.Add("img1", new Vector2(20f, 30f));
        imageController3.setSnapOffsets(offsets3);
        List<GameObject> snapPoints3 = new List<GameObject>();
        snapPoints3.Add(image1);
        snapPoints3.Add(image2);
        imageController3.setSnapPoints(snapPoints3);
        List<GameObject> snapPoints2 = new List<GameObject>();
        snapPoints2.Add(image1);
        snapPoints2.Add(image3);
        imageController2.setSnapPoints(snapPoints2);
        List<GameObject> snapPoints = new List<GameObject>();
        snapPoints.Add(image2);
        snapPoints.Add(image3);
        imageController.setSnapPoints(snapPoints);

        // Act
        bool result = gameController.isAllSnapPointsEqual(image1);

        // Assert
        Assert.IsTrue(result);
    }

    // updateSnapPositions updates snap points for all images except the moved one
    // test runs fine, but does produce a null object error after due to partial mocking
    [Test]
    public void test_update_snap_positions()
    {
        // Arrange
        List<GameObject> images = new List<GameObject>();
        GameObject gameObject= new GameObject();
        ImagerGameController gameController = gameObject.AddComponent<ImagerGameController>();

        GameObject imageMoved = new GameObject("img1");
        SnapToTarget snapToTarget = imageMoved.AddComponent<SnapToTarget>();
        RectTransform rectTransform = imageMoved.AddComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(100f, 100f);
        ImageController imageController = imageMoved.AddComponent<ImageController>();
        Dictionary<string, Vector2> offsets = new Dictionary<string, Vector2>();
        offsets.Add("img2", new Vector2(-10f, -10f));
        imageController.setSnapOffsets(offsets);
        GameObject image2 = new GameObject("img2");
        RectTransform rectTransform2 = image2.AddComponent<RectTransform>();
        rectTransform2.anchoredPosition = new Vector2(150f, 150f);
        ImageController imageController2 = image2.AddComponent<ImageController>();
        Dictionary<string, Vector2> offsets2 = new Dictionary<string, Vector2>();
        offsets2.Add("img1", new Vector2(10f, 10f));
        imageController2.setSnapOffsets(offsets2);
        List<GameObject> snapPoints2 = new List<GameObject>();
        snapPoints2.Add(imageMoved);
        imageController2.setSnapPoints(snapPoints2);
        List<GameObject> snapPoints = new List<GameObject>();
        snapPoints.Add(image2);
        imageController.setSnapPoints(snapPoints);

        rectTransform.anchoredPosition = new Vector2(90f, 90f);

        // Set up game controller
        images.Add(imageMoved);
        images.Add(image2);
        gameController.images = images;

        // Act
        gameController.updateSnapPositions(imageMoved);

        // Assert
        ImageController imgController = image2.GetComponent<ImageController>();

        float tolerance = 0.0001f; // Define your tolerance value here
        Vector2 imageMovedPosition = imageMoved.GetComponent<RectTransform>().anchoredPosition;
        Vector2[] valuesArray = new Vector2[imgController.getSnapPoints().Count];
        imgController.getSnapPoints().CopyTo(valuesArray, 0);
        Vector2 snapPoint0 = valuesArray[0];

        bool isWithinToleranceOfSnapPoint0 = Vector2.Distance(imageMovedPosition + new Vector2(10f, 10f), snapPoint0) < tolerance;

        Assert.IsTrue(isWithinToleranceOfSnapPoint0, "imageMoved's anchoredPosition is not equal to either snapPoint0");
    }

    // CalcScore calculates score based on time
    // test runs fine, but does produce a null object error after due to partial mocking
    [Test]
    public void test_calc_score()
    {
        // Arrange
        GameObject gameObject = new GameObject();
        ImagerGameController gameController = gameObject.AddComponent<ImagerGameController>();
        gameController.timer = gameObject.AddComponent<GameTimer>();
        gameController.timer.setTime(30.0f);

        // Act
        gameController.CalcScore();

        // Assert
        Assert.AreEqual(7292, gameController.score);
    }
}
