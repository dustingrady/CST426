Status: 

Project currently reads in values from a webcam texture, breaks the image into a series of 'pixel blocks' adds these blocks to a flattened 2D array and returns the average RGB values on a per-block basis.

The blocks are then compared to all surrounding neighbors, if there is enough of a difference between surrounding RGB values from the current node (with tolerance factored in), the current node becomes an object candidate.

