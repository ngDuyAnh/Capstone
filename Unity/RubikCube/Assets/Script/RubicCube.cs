using System.Collections.Generic;
using System;
using UnityEngine.UIElements;
using Unity.VisualScripting;
using System.Diagnostics;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public enum Face
{
    Front,
    Right,
    Back,
    Left,
    Top,
    Bottom
}

public enum SquareColour
{
    White,
    Yellow,
    Green,
    Blue,
    Red,
    Orange
}

public class Square
{
    public SquareColour Color { get; private set; }

    public Square(SquareColour color)
    {
        Color = color;
    }
}

public class RubikFace
{
    // Public member
    private List<List<Square>> squareList;

    // Public method
    public RubikFace(int size, SquareColour colour)
    {
        // Create the squares of the face
        squareList = new List<List<Square>>();
        for (int row = 0; row < size; row++)
        {
            // Create the row
            List<Square> rowList = new List<Square>();
            squareList.Add(rowList);

            // Initialize the squares of the row
            for (int col = 0; col < size; col++)
            {
                rowList.Add(new Square(colour));
            }
        }
    }

    public List<List<Square>> getRubikFace()
    {
        return squareList;
    }

    public int GetSize()
    {
        return squareList.Count;
    }

    public List<Square> GetRow(int row)
    {
        return new List<Square>(squareList[row]);
    }

    public List<Square> GetCol(int col)
    {
        // Get the element in the column
        List<Square> colList = new List<Square>();
        for (int row = 0; row < squareList.Count; row++)
        {
            colList.Add(squareList[row][col]);
        }

        // Return
        return colList;
    }

    public void SetSquare(Square square, int row, int col)
    {
        Debug.Assert(row < GetSize(), "The index must be within bound.");
        Debug.Assert(col < GetSize(), "The index must be within bound.");

        // Set the square to the given index
        squareList[row][col] = square;
    }

    public void SetRow(List<Square> list, int row)
    {
        Debug.Assert(row < GetSize(), "The index must be within bound.");

        // Set the row
        for (int counter = 0; counter < list.Count; counter++)
        {
            squareList[row][counter] = list[counter];
        }
    }

    public void SetCol(List<Square> list, int col)
    {
        Debug.Assert(col < GetSize(), "The index must be within bound.");

        // Set the row
        for (int counter = 0; counter < list.Count; counter++)
        {
            squareList[counter][col] = list[counter];
        }
    }

    public void RotateClockwise()
    {
        int size = squareList.Count;
        List<List<Square>> result = new List<List<Square>>();
        for(int row = 0; row < size; row++)
        {
            List<Square> rowResult = new List<Square>();
            for (int col = 0; col < size; col++)
            {
                rowResult.Add(squareList[size - 1 - col][row]);
            }
        }

        // Intialize with the result
        squareList = result;
    }

    public void RotateCounterClockwise()
    {
        int size = squareList.Count;
        List<List<Square>> result = new List<List<Square>>();
        for (int row = 0; row < size; row++)
        {
            List<Square> rowResult = new List<Square>();
            for (int col = 0; col < size; col++)
            {
                rowResult.Add(squareList[col][size - 1 - row]);
            }
        }

        // Intialize with the result
        squareList = result;
    }
}

public class RubikCube
{
    // Private static method
    private static SquareColour GetDefaultColor(Face face)
    {
        switch (face)
        {
            case Face.Top:
                return SquareColour.White;
            case Face.Bottom:
                return SquareColour.Yellow;
            case Face.Left:
                return SquareColour.Green;
            case Face.Right:
                return SquareColour.Blue;
            case Face.Front:
                return SquareColour.Red;
            case Face.Back:
                return SquareColour.Orange;
            default:
                throw new InvalidOperationException("Invalid cube face.");
        }
    }

    // Private member
    private Dictionary<Face, RubikFace> faceList; // Each face will have rows of sub cube face
    private Face currentFrontFace;

    // Public method
    public RubikCube(int size)
    {
        // The default front face is the front
        currentFrontFace = Face.Front;

        // Create the faces of the rubik cube
        faceList = new Dictionary<Face, RubikFace>();
        foreach (Face face in Enum.GetValues(typeof(Face)))
        {
            faceList[face] = new RubikFace(size, RubikCube.GetDefaultColor(face));
        }
    }

    public void RotateRowRight(Face face, int row)
    {
        // Set the view parameter
        // This is to generalize the algorithmn
        List<Face> sideView = new List<Face> { Face.Front, Face.Right, Face.Back, Face.Left };
        Face top = Face.Top, bottom = Face.Bottom;

        // The view is either top or bottom, then we have to change the view accordingly
        if (face == Face.Top || face == Face.Bottom)
        {
            /*horizontalView = new List<Face> { Face.Top, Face.Right, Face.Bottom, Face.Left };
            top = Face.Back;
            bottom = Face.Front;

            // Rotate to use the row indexing
            faceList[Face.Right].RotateCounterClockwise();
            //faceList[Face.Left]

            // The front will have to be the right following the standard of left to right, top to bottom
            Reorient(Face.Right);

            *//*horizontalView = new List<CubeFace> { CubeFace.Top, CubeFace.Right, CubeFace.Bottom, CubeFace.Left };
            top = CubeFace.Back;
            bottom = CubeFace.Front;*//*

            // Perform the rotation of up to down with respect to the new orientation

            // Change the orientation back to default
            Reorient(Face.Front);*/

            // Change the orientation of the right side to be the front
            // This will allow us to take advantage and resuse the column operation
            Reorient(Face.Right);

            // Perform the rotation
            RotateColumnDown

            // Change the cube back to the default orientation
            Reorient(Face.Front);
        }
        // Perform the rotation across
        else
        {
            PerformRotationRow(sideView, top, bottom, row);
        }
    }

    public void RotateRowLeft(Face face, int row)
    {
        // Set the view parameter
        // This is to generalize the algorithmn
        List<Face> sideView = new List<Face> { Face.Back, Face.Left, Face.Front, Face.Right };
        Face top = Face.Bottom, bottom = Face.Top;

        // The view is either top or bottom, then we have to change the view accordingly
        if (face == Face.Top || face == Face.Bottom)
        {
            sideView = new List<Face> { Face.Top, Face.Left, Face.Bottom, Face.Right };
            top = Face.Front;
            bottom = Face.Back;
        }

        // Perform the rotation across
        PerformRotationRow(sideView, top, bottom, row);
    }

    public void RotateColumnDown(Face face, int col)
    {
        // The view is either top or bottom, then we have to change the view accordingly
        if (face == CubeFace.Right || face == CubeFace.Left)
        {
            /*horizontalView = new List<CubeFace> { CubeFace.Right, CubeFace.Bottom, CubeFace.Left, CubeFace.Top };
            top = CubeFace.Front;
            bottom = CubeFace.Back;*/
        }
        // Perform the rotation top to bottom
        else
        {
            List<Face> sideView = new List<Face> { Face.Front, Face.Bottom, Face.Back, Face.Top };
            PerformRotationRow(sideView, Face.Left, Face.Right, col);
        }
    }

    // Private method

    private void Reorient(Face targetFrontFace)
    {
        // Keep rotate to the whole rubik cube until the front is the given face
        for (int counter = 0; (Face)(((int)currentFrontFace + counter) % 4) != targetFrontFace; counter++)
        {
            // Reorient the cube
            for (int row = 0; row < faceList[Face.Front].GetSize(); row++)
            {
                this.PerformRotationRow(sideView, top, bottom, row);
            }

            // Update the front face as it has changed during the rotation
            currentFrontFace = (Face)(((int)currentFrontFace + 1) % 4);
        }
    }

    private void PerformRotationRow(int row)
    {
        // Local variable dictionary
        List<Face> horizontalView = new List<Face> { Face.Front, Face.Right, Face.Back, Face.Left };
        Face top = Face.Top, bottom = Face.Bottom;

        // Perform the rotation of the top or bottom
        if (row == 0)
        {
            faceList[top].RotateCounterClockwise();
        }
        else if (row == (faceList[Face.Front].GetSize() - 1))
        {
            faceList[bottom].RotateClockwise();
        }

        // Rotate will be to the right
        List<Square> tempRow = faceList[horizontalView[horizontalView.Count - 1]].GetRow(row);
        for (int counter = 0; counter < horizontalView.Count; ++counter) 
        {
            // Get the current face of operation
            Face currentFace = horizontalView[counter];
            
            // Replace the current face to this
            List<Square> replaceSource = tempRow;

            // Save a copy of the current before the replacing
            tempRow = faceList[currentFace].GetRow(row);

            // Perform the replacement
            faceList[currentFace].SetRow(replaceSource, row);
        }
    }

    private void PerformRotationCol(List<Face> verticalView, Face left, Face right, int col)
    {
        // Local variable dictionary
        List<Face> sideView = new List<Face> { Face.Top, Face.Front, Face.Bottom, Face.Back };
        Face top = Face.Top, bottom = Face.Bottom;

        /*
        Generalize the rotation will be from the top to bottom
        */
        Debug.Assert(verticalView.Count == 4, "Default implementation is 4 faces for side.");
        Debug.Assert(col < verticalView.Count, "The index must be within bound.");

        // Perform the rotation of the top or bottom
        if (col == 0)
        {
            faceList[left].RotateClockwise();
        }
        else if (col == (faceList[Face.Front].GetSize() - 1))
        {
            faceList[right].RotateCounterClockwise();
        }

        // Rotate will be top to bottom
        List<Square> tempCol = faceList[verticalView[verticalView.Count - 1]].GetCol(col);
        for (int counter = 0; counter < verticalView.Count; ++counter)
        {
            // Get the current face of operation
            int changeCol = col;
            Face currentFace = verticalView[counter];

            // Replace the current face to this
            List<Square> replaceSource = tempCol;

            // Save a copy of the current before the replacing
            tempCol = faceList[currentFace].GetCol(col);

            // If the current face is the back
            // Because of the direction of rotation
            // We will need to insert it the revert order
            if (currentFace == Face.Back)
            {
                changeCol = (faceList[Face.Front].GetSize() - 1) - col;
                replaceSource.Reverse();
            }

            // Perform the replacement
            faceList[currentFace].SetCol(replaceSource, changeCol);
        }
    }
}
