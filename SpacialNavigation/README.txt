Status: 

Project currently reads in values from a webcam texture, breaks the image into a series of 'pixel blocks' adds these blocks to a flattened 2D array and returns the average RGB values on a per-block basis.

The next step is to compare neighboring pixel-blocks, look for similar RGB values and return how many share a similar color. This will give us an idea of how large the object is, and how big the 3D model that we are going to insert in unity should be.

After working on this prototype, I have discovered the z-buffer which is probably a better approach to solving the problem.