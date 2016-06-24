using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;


/// <summary>
/// Summary description for WaterMark
/// </summary>
public class WaterMark
{
	public WaterMark()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public static void CreateImage(string ImputFile,string OutputFile,string Text)
    {
        //define a string of text to use as the Copyright message
        string Copyright = Text;;

        //create a image object containing the photograph to watermark
        Image imgPhoto = Image.FromFile(ImputFile);
        int phWidth = imgPhoto.Width;
        int phHeight = imgPhoto.Height;

        //create a Bitmap the Size of the original photograph
        Bitmap bmPhoto = new Bitmap(phWidth, phHeight, PixelFormat.Format24bppRgb);

        bmPhoto.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);

        //load the Bitmap into a Graphics object 
        Graphics grPhoto = Graphics.FromImage(bmPhoto);

        //create a image object containing the watermark
        //Image imgWatermark = new Bitmap(WorkingDirectory + "\\watermark.bmp");
        //int wmWidth = imgWatermark.Width;
        //int wmHeight = imgWatermark.Height;

        //------------------------------------------------------------
        //Step #1 - Insert Copyright message
        //------------------------------------------------------------

        //Set the rendering quality for this Graphics object
        grPhoto.SmoothingMode = SmoothingMode.AntiAlias;

        //Draws the photo Image object at original size to the graphics object.
        grPhoto.DrawImage(
            imgPhoto,                               // Photo Image object
            new Rectangle(0, 0, phWidth, phHeight), // Rectangle structure
            0,                                      // x-coordinate of the portion of the source image to draw. 
            0,                                      // y-coordinate of the portion of the source image to draw. 
            phWidth,                                // Width of the portion of the source image to draw. 
            phHeight,                               // Height of the portion of the source image to draw. 
            GraphicsUnit.Pixel);                    // Units of measure 

        //-------------------------------------------------------
        //to maximize the size of the Copyright message we will 
        //test multiple Font sizes to determine the largest posible 
        //font we can use for the width of the Photograph
        //define an array of point sizes you would like to consider as possiblities
        //-------------------------------------------------------
        int[] sizes = new int[] { 16, 14, 12, 10, 8, 6, 4 };

        Font crFont = null;
        SizeF crSize = new SizeF();
        
        //Loop through the defined sizes checking the length of the Copyright string
        //If its length in pixles is less then the image width choose this Font size.
        for (int i = 0; i < 7; i++)
        {
            //set a Font object to Arial (i)pt, Bold
            crFont = new Font("arial", sizes[i], FontStyle.Bold);
            //Measure the Copyright string in this Font
            crSize = grPhoto.MeasureString(Copyright, crFont);

            if ((ushort)crSize.Width < (ushort)phWidth)
                break;
        }

        //Since all photographs will have varying heights, determine a 
        //position 5% from the bottom of the image
        int yPixlesFromBottom = (int)(phHeight * .05);

        //Now that we have a point size use the Copyrights string height 
        //to determine a y-coordinate to draw the string of the photograph
        float yPosFromBottom = ((phHeight - yPixlesFromBottom) - (crSize.Height / 2));

        //Determine its x-coordinate by calculating the center of the width of the image
        float xCenterOfImg = (phWidth / 2);

        //Define the text layout by setting the text alignment to centered
        StringFormat StrFormat = new StringFormat();
        StrFormat.Alignment = StringAlignment.Center;

        //define a Brush which is semi trasparent black (Alpha set to 153)
        SolidBrush semiTransBrush2 = new SolidBrush(Color.FromArgb(153, 0, 0, 0));

        //Draw the Copyright string
        grPhoto.DrawString(Copyright,                 //string of text
            crFont,                                   //font
            semiTransBrush2,                           //Brush
            new PointF(xCenterOfImg + 1, yPosFromBottom + 1),  //Position
            StrFormat);

        //define a Brush which is semi trasparent white (Alpha set to 153)
        SolidBrush semiTransBrush = new SolidBrush(Color.FromArgb(153, 255, 255, 255));

        //Draw the Copyright string a second time to create a shadow effect
        //Make sure to move this text 1 pixel to the right and down 1 pixel
        grPhoto.DrawString(Copyright,                 //string of text
            crFont,                                   //font
            semiTransBrush,                           //Brush
            new PointF(xCenterOfImg, yPosFromBottom),  //Position
            StrFormat);                               //Text alignment

        //------------------------------------------------------------
        //Step #2 - Insert Watermark image
        //------------------------------------------------------------

        //Create a Bitmap based on the previously modified photograph Bitmap
        Bitmap bmWatermark = new Bitmap(bmPhoto);
        bmWatermark.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);
        //Load this Bitmap into a new Graphic Object
        Graphics grWatermark = Graphics.FromImage(bmWatermark);

        //To achieve a transulcent watermark we will apply (2) color 
        //manipulations by defineing a ImageAttributes object and 
        //seting (2) of its properties.
        ImageAttributes imageAttributes = new ImageAttributes();

        //The first step in manipulating the watermark image is to replace 
        //the background color with one that is trasparent (Alpha=0, R=0, G=0, B=0)
        //to do this we will use a Colormap and use this to define a RemapTable
        ColorMap colorMap = new ColorMap();

        //My watermark was defined with a background of 100% Green this will
        //be the color we search for and replace with transparency
        colorMap.OldColor = Color.FromArgb(255, 0, 255, 0);
        colorMap.NewColor = Color.FromArgb(0, 0, 0, 0);

        ColorMap[] remapTable = { colorMap };

        imageAttributes.SetRemapTable(remapTable, ColorAdjustType.Bitmap);

        //The second color manipulation is used to change the opacity of the 
        //watermark.  This is done by applying a 5x5 matrix that contains the 
        //coordinates for the RGBA space.  By setting the 3rd row and 3rd column 
        //to 0.3f we achive a level of opacity
        float[][] colorMatrixElements = { 
												new float[] {1.0f,  0.0f,  0.0f,  0.0f, 0.0f},       
												new float[] {0.0f,  1.0f,  0.0f,  0.0f, 0.0f},        
												new float[] {0.0f,  0.0f,  1.0f,  0.0f, 0.0f},        
												new float[] {0.0f,  0.0f,  0.0f,  0.3f, 0.0f},        
												new float[] {0.0f,  0.0f,  0.0f,  0.0f, 1.0f}};
        ColorMatrix wmColorMatrix = new ColorMatrix(colorMatrixElements);

        imageAttributes.SetColorMatrix(wmColorMatrix, ColorMatrixFlag.Default,
            ColorAdjustType.Bitmap);

        //For this example we will place the watermark in the upper right
        //hand corner of the photograph. offset down 10 pixels and to the 
        //left 10 pixles

        //int xPosOfWm = ((phWidth - wmWidth) - 10);
        //int yPosOfWm = 10;

        //grWatermark.DrawImage(imgWatermark,
        //    new Rectangle(xPosOfWm, yPosOfWm, wmWidth, wmHeight),  //Set the detination Position
        //    0,                  // x-coordinate of the portion of the source image to draw. 
        //    0,                  // y-coordinate of the portion of the source image to draw. 
        //    wmWidth,            // Watermark Width
        //    wmHeight,		    // Watermark Height
        //    GraphicsUnit.Pixel, // Unit of measurment
        //    imageAttributes);   //ImageAttributes Object

        //Replace the original photgraphs bitmap with the new Bitmap
        imgPhoto = bmWatermark;
        grPhoto.Dispose();
        grWatermark.Dispose();

        //save new image to file system.
        imgPhoto.Save(OutputFile, ImageFormat.Jpeg);
        imgPhoto.Dispose();
        //imgWatermark.Dispose();
   }
}