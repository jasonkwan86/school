#include <stdio.h>
#include <stdlib.h>
#include "main.h"
#include "libpnm.h"
#include <string.h>

int MAX_COLOR = 255;

int min(int a, int b) { return a < b ? a : b; }
int max(int a, int b) { return a > b ? a : b; }

// pbm - program 1
int program1(ImageProps props)
{
    if (!isDimensionValid4(props.width))
    {
        return showErrorMessage("Width must be a multiple of 4 and at least 4.");
    }
    struct PBM_Image img;
    if (create_PBM_Image(&img, props.width, props.height))
    {
        return showErrorMessage("Failed to create PBM image.");
    }
    int quarterWidth = props.width / 4;
    int quarterHeight = props.height / 4;

    // Remember pbm is 0 = white by default
    for (int row = 0; row < props.height; row++)
    {
        for (int col = 0; col < props.width; col++)
        {
            // When to draw pixel black
            if (row < quarterHeight || row >= quarterHeight * 3 || col < quarterWidth || col >= quarterWidth * 3)
            {
                img.image[row][col] = 1;
            }
        }
    }

    // top left to bottom left slope
    int x1 = quarterWidth, x2 = quarterWidth * 3, y1 = quarterHeight, y2 = quarterHeight * 3;
    float slope = (float)(y2 - y1) / (x2 - x1);
    int yIntercept = y1 - slope * x1;

    // iterate over height
    if (props.height > props.width)
    {
        for (int y = min(y1, y2); y < max(y1, y2); y++)
        {
            int x = (y - yIntercept) / slope;
            img.image[y][x] = 1;
            img.image[y][props.width - x - 1] = 1;
        }
    }
    // iterate over width
    else
    {
        for (int x = x1; x < x2; x++)
        {
            int y = slope * x + yIntercept;
            img.image[y][x] = 1;
            img.image[props.height-y - 1][x] = 1;
        }
    }

    if (save_PBM_Image(&img, props.fileName, props.raw))
    {
        return showErrorMessage("Failed to save PBM image.");
    }
    free_PBM_Image(&img);

    return 0;
}

// pgm - program 2
int program2(ImageProps props)
{
    if (!isDimensionValid4(props.width))
    {
        return showErrorMessage("Width must be a multiple of 4 and at least 4.");
    }
    struct PGM_Image img;
    if (create_PGM_Image(&img, props.width, props.height, 255))
    {
        return showErrorMessage("Failed to create PGM image.");
    }
    int quarterWidth = props.width / 4;
    int quarterHeight = props.height / 4;
    int halfWidth = props.width / 2;
    int halfHeight = props.height / 2;

    float slope = (float)props.height / props.width;

    // for vertical rectangles, loop over height
    if (slope > 1)
    {
        for (int absRow = quarterHeight; absRow < halfHeight; absRow++)
        {
            int absY = abs(absRow - halfHeight);
            int col = absRow / slope;
            int grayLevel = absY * MAX_COLOR / quarterHeight;

            // fill out this row, as well as its mirror pixel on the x-axis
            for (int row = absRow; row < props.height - absRow; row++)
            {
                img.image[row][col] = grayLevel;
                img.image[row][props.width - col - 1] = grayLevel;
            }
            // fill out this column, as well as its mirror pixel on the y-axis
            for (int c = col; c < props.width - col; c++)
            {
                img.image[absRow][c] = grayLevel;
                img.image[props.height - absRow - 1][c] = grayLevel;
            }
        }
    }
    // for horizontal rectangles, loop over width
    else
    {
        for (int absCol = quarterWidth; absCol < halfWidth; absCol++)
        {
            int absX = abs(absCol - halfWidth);
            int grayLevel = absX * MAX_COLOR / quarterWidth;
            int row = slope * absCol;

            // top and bottom triangles
            for (int col = absCol; col < props.width - absCol; col++)
            {
                img.image[row][col] = grayLevel;
                img.image[props.height - row - 1][col] = grayLevel;
            }

            // left and right triangles
            for (int r = row; r < props.height - row; r++)
            {
                img.image[r][absCol] = grayLevel;
                img.image[r][props.width - absCol - 1] = grayLevel;
            }
        }
    }

    if (save_PGM_Image(&img, props.fileName, props.raw))
    {
        return showErrorMessage("Failed to save PGM image.");
    }
    free_PGM_Image(&img);

    return 0;
}

// ppm - program 3
int program3(ImageProps props)
{
    if (!isDimensionValid6(props.width))
    {
        return showErrorMessage("Width must be a multiple of 6 and at least 6.");
    }

    struct PPM_Image img;
    if (create_PPM_Image(&img, props.width, props.height, MAX_COLOR))
    {
        return showErrorMessage("Failed to create PPM image.");
    }

    int thirdWidth = props.width / 3;
    int halfHeight = props.height / 2;
    int halfWidth = props.width / 2;

    // Remember image is 0 = white by default
    for (int row = 0; row < props.height; row++)
    {
        for (int col = 0; col < props.width; col++)
        {
            if (row < halfHeight)
            {
                // Red gradient
                if (col < thirdWidth)
                {
                    int gradient = (unsigned char)(row * MAX_COLOR / halfHeight);
                    // green and blue are 0 at row = 0, and max color at row = halfHeight
                    img.image[row][col][RED] = MAX_COLOR;
                    img.image[row][col][GREEN] = gradient;
                    img.image[row][col][BLUE] = gradient;
                }
                // Green gradient
                else if (col < thirdWidth * 2)
                {
                    int gradient = (unsigned char)(MAX_COLOR - (row * MAX_COLOR / halfHeight));
                    // green and blue are 255 at row = 0, and 255 at row = halfHeight
                    img.image[row][col][RED] = gradient;
                    img.image[row][col][GREEN] = MAX_COLOR;
                    img.image[row][col][BLUE] = gradient;
                }
                // Blue gradient
                else
                {
                    int gradient = (unsigned char)(row * MAX_COLOR / halfHeight);
                    // red and green are 0 at row = 0, and max color at row = halfHeight
                    img.image[row][col][RED] = gradient;
                    img.image[row][col][GREEN] = gradient;
                    img.image[row][col][BLUE] = MAX_COLOR;
                }
            }
            else
            {
                // gray scales
                int gradient = col < halfWidth ? (unsigned char)((row - halfHeight) * MAX_COLOR / halfHeight) : (unsigned char)(MAX_COLOR - ((row - halfHeight) * MAX_COLOR / halfHeight));
                img.image[row][col][RED] = gradient;
                img.image[row][col][GREEN] = gradient;
                img.image[row][col][BLUE] = gradient;
            }
        }
    }

    struct PGM_Image redImg, greenImg, blueImg;
    if (save_PPM_Image(&img, props.fileName, props.raw))
    {
        return showErrorMessage("Failed to save PPM image.");
    }
    if (copy_PPM_to_PGM(&img, &redImg, RED) != 0) {
        return showErrorMessage("Failed to copy red PPM to PGM image.");
    }
    if (copy_PPM_to_PGM(&img, &greenImg, GREEN) != 0) {
        return showErrorMessage("Failed to copy green PPM to PGM image.");
    }
    if (copy_PPM_to_PGM(&img, &blueImg, BLUE) != 0) {
        return showErrorMessage("Failed to copy blue PPM to PGM image.");
    }
    if (save_PGM_Image(&redImg, "program_3_red.pgm", props.raw) != 0)
    {
        return showErrorMessage("Failed to save red PGM image.");
    }
    if (save_PGM_Image(&greenImg, "program_3_green.pgm", props.raw) != 0)
    {
        return showErrorMessage("Failed to save green PGM image.");
    }
    if (save_PGM_Image(&blueImg, "program_3_blue.pgm", props.raw) != 0)
    {
        return showErrorMessage("Failed to save blue PGM image.");
    }
    free_PPM_Image(&img);

    return 0;
}

bool isDimensionValid4(int dimension)
{
    return isDimensionValid(dimension, 4, 4);
}

bool isDimensionValid6(int dimension)
{
    return isDimensionValid(dimension, 6, 6);
}

// 0 if valid, 1 if invalid
bool isDimensionValid(int dimension, int modulus, int min)
{
    if (dimension % modulus != 0)
    {
        return false;
    }
    if (dimension < min)
    {
        return false;
    }
    return true;
}

int showErrorMessage(char *message)
{
    printf("%s\n", message);
    return 0;
}

int main(int argc, char *argv[])
{
    if (argc != 6)
    {
        printf("There must be 5 arguments:\n");
        printf("1: <typeCode> (1, 2, or 3)\n");
        printf("2: <width> (in pixels)\n");
        printf("3: <height> (in pixels)\n");
        printf("4: <fileName> (with extension)\n");
        printf("5: <raw> (0 or 1)\n");
        
        return 0;
    }

    int typeCode = atoi(argv[1]);
    int width = atoi(argv[2]);
    int height = atoi(argv[3]);
    char * fileName = argv[4];
    bool raw = (bool)atoi(argv[5]);

    ImageProps props = {width, height, raw, fileName};
    
    if (width <= 0 || height <= 0)
    {
        return showErrorMessage("Width and height must be positive.");
    }
    if (fileName == NULL || strlen(fileName) == 0)
    {
        return showErrorMessage("File name must be non-empty.");
    }

    if (!isDimensionValid4(height))
    {
        return showErrorMessage("Height must be a multiple of 4 and at least 4.");
    }

    switch (typeCode)
    {
        case 1:
            return program1(props);
        case 2:
            return program2(props);
        case 3:
            return program3(props);
        default:
            return showErrorMessage("Type code must be 1, 2, or 3.");
    }

    return 0;
}