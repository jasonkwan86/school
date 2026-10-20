#include "libpnm.h"
#include "lz77_decoding_function.h"

int main(int argc, char *argv[])
{
    if (argc != 2)
    {
        printf("There must be 1 argument:\n");
        printf("1: <input filename> ()\n");
        
        return 0;
    }

    char *compressedFileName = argv[1];

    // Main function must report the decompression time
    Decode_Using_LZ77(compressedFileName);

    return 0;
}
