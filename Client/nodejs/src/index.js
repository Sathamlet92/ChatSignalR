import { initializeApp } from "firebase/app";
import { getStorage, ref, uploadBytesResumable, getDownloadURL } from "firebase/storage"
import { getAuth, signInWithEmailAndPassword } from "firebase/auth";

export  const UploadFile = async (imageDto, configs) => {
  
   try {
   console.log(configs);
    const firebaseApp = initializeApp(configs.firebaseConfig);
    const auth = getAuth(firebaseApp);
    const storage = getStorage(firebaseApp);
    const userCredential = await signInWithEmailAndPassword(auth, configs.firebaseAuth.email, configs.firebaseAuth.authPassword);
    const storageRef = ref(storage, `${imageDto.folderName}/${imageDto.imageName}`);
    const uploadTask = uploadBytesResumable(storageRef, imageDto.imageData);

    uploadTask.on("state_change", (snapshot) => {
      const progress = (snapshot.bytesTransferred / snapshot.totalBytes)  * 100;
      console.log('Upload is ' + progress + '% done');
    }, (error) =>{
      console.log(error)
    }, () => {
      console.log("Se subio pa!")
    });
    const imageUrl = await getDownloadURL(storageRef);
    return imageUrl;
   } catch (error) {
    console.error("Error:", error);
    return error;    
   }
}

