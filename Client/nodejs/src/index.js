import { initializeApp } from "firebase/app";
import { getStorage, ref, uploadBytesResumable, getDownloadURL } from "firebase/storage"
import { getAuth, signInWithEmailAndPassword } from "firebase/auth";

export  const uploadFile = async (imageDto, configs, profileDotnet) => {  
   try {
    const firebaseApp = initializeApp(configs.firebaseConfig);
    const auth = getAuth(firebaseApp);
    const storage = getStorage(firebaseApp);
    const userCredential = await signInWithEmailAndPassword(auth, configs.firebaseAuth.email, configs.firebaseAuth.authPassword);
    const storageRef = ref(storage, `${imageDto.folderName}/${imageDto.imageName}`);
    const metadata = {
      contentType: imageDto.contentType
    };    
    const uploadTask = uploadBytesResumable(storageRef, imageDto.imageData, metadata);

    uploadTask.on("state_change", async (snapshot) => {
      const progress = (snapshot.bytesTransferred / snapshot.totalBytes)  * 100;
      await profileDotnet.invokeMethodAsync("UpdateProgress", Math.round(progress));
    }, (error) =>{
      console.log(error)
    }, () => {
      console.log('se subio papu')
    });
    const imageUrl = await getDownloadURL(storageRef);
    console.log(imageUrl);
    return imageUrl;
   } catch (error) {
    return error;    
   }
}

export const saveProfileSession = (data) => {
  sessionStorage.setItem(data.key, data.value);
}

export const getProfileSession = (key) => {
  return sessionStorage.getItem(key);
}

