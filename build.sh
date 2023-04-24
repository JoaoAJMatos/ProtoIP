while true; do
      echo "Select build type:"
      echo "1. Build library"
      echo "2. Build and run an example program"
      read -p "-> " choice

      if [ $choice -eq 1 ]; then
            mcs -target:library -out:build/protoip.dll /reference:System.Numerics.dll src/*.cs
            break
      elif [ $choice -eq 2 ]; then
            read -p "Enter the name of the example program (without file extension): " example
            mcs -out:build/$example.exe /reference:System.Numerics.dll /reference:build/protoip.dll examples/$example.cs
            sudo mono build/$example.exe
            break
      else
            echo "Invalid choice"
      fi
done
