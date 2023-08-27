import { initializeApp } from "firebase/app";
import { getStorage, ref, uploadString, getDownloadURL } from "firebase/storage"
import { getAuth, signInWithEmailAndPassword } from "firebase/auth";

export  const UploadFile = async (imageDto) => {
  
   try {
    console.log(imageDto);
       // Your web app's Firebase configuration
    const firebaseConfig = {
      apiKey: "AIzaSyDA2MdV2F06uKAJ5r5BKTb-Qw5j_p0eXug",
      authDomain: "chat-empresarial-blazor.firebaseapp.com",
      projectId: "chat-empresarial-blazor",
      storageBucket: "chat-empresarial-blazor.appspot.com",
      messagingSenderId: "945940315321",
      appId: "1:945940315321:web:1519de591572273104ef68"
    };
    const email = 'sathamlet@gmail.com';
    const password = 'YueSol0410';

    const firebaseApp = initializeApp(firebaseConfig);
    const auth = getAuth(firebaseApp);
    const storage = getStorage(firebaseApp);
    const userCredential = await signInWithEmailAndPassword(auth, email, password);
    const storageRef = ref(storage, `${imageDto.folderName}/${imageDto.imageName}`);
    const result = await uploadString(storageRef, imageDto.imageBase64, 'data_url');
    const imageUrl = await getDownloadURL(storageRef);
    return imageUrl;
   } catch (error) {
    console.error("Error:", error);
    return error;    
   }
}

