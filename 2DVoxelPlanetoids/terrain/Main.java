package com.company;


import javax.imageio.ImageIO;
import java.awt.*;
import java.awt.image.BufferedImage;
import java.io.*;
import java.nio.file.Files;


public class Main {

    static final int rowsPerChunk = 100;
    static final int colsPerChunk = 100;
    static final int chunksPerCol = 3;
    static final int chunksPerRow = 10;
    static final int imageWidth = colsPerChunk * chunksPerRow;
    static final int imageHeight = rowsPerChunk * chunksPerCol;
    static final Color[] colors = new Color[]{Color.BLUE,Color.GRAY,Color.GREEN,Color.ORANGE,Color.PINK};
    public static void main(String[] args) throws IOException {
	    if(args.length != 2)
            System.out.println("Input .dat file path and output file");

        byte[] bytes = new byte[0];
        try {
            bytes = Files.readAllBytes(new File(args[0]).toPath());
        } catch (IOException e) {
            System.out.println("Could not find file " + e.getMessage());
        }

        BufferedImage image = new BufferedImage(imageWidth,imageHeight,Image.SCALE_DEFAULT);
        Graphics2D g2 = image.createGraphics();
        for(int chunkY = 0; chunkY < chunksPerCol; chunkY ++ ){
            for(int chunkX = 0; chunkX < chunksPerRow; chunkX ++){
                for(int y = 0; y < rowsPerChunk; y ++){
                    for(int x = 0; x < colsPerChunk; x ++){
                        g2.setColor(colors[bytes[chunkY * chunksPerRow * colsPerChunk * rowsPerChunk +
                                chunkX * colsPerChunk * rowsPerChunk + y * colsPerChunk + x]]);
                        g2.fillRect(colsPerChunk * chunkX + x,rowsPerChunk* chunkY + y,1,1);
                    }
                }
            }
        }
        ImageIO.write(image,"BMP",new File(args[1]));
    }
}
