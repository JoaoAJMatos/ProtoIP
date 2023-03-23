while true; do
      echo "Select build type:"
      echo "1. Build library"
      echo "2. Build and run executable"
      read -p "-> " choice

      if [ $choice -eq 1 ]; then
            mcs -target:library -out:build/protoip.dll /reference:System.Numerics.dll src/*.cs
            break
      elif [ $choice -eq 2 ]; then
            mcs -out:build/protoip.exe /reference:System.Numerics.dll src/*.cs
            mono build/protoip.exe
            break
      else
            echo "Invalid choice"
      fi
done