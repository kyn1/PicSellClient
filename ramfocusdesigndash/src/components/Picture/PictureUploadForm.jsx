import React, { useState } from 'react';
import axios from 'axios';
import PictureCreateForm from './PictureCreateForm';

function PictureUploadForm() {
  const [folderName, setFolderName] = useState('');
  const [selectedFiles, setSelectedFiles] = useState([]);
  const [uploading, setUploading] = useState(false);
  const [uploadResponse, setUploadResponse] = useState(null);
  const [error, setError] = useState(null);
  const [proceedToCreate, setProceedToCreate] = useState(false);

  const handleFileChange = (e) => {
    setSelectedFiles(e.target.files);
  };

  const handleFolderNameChange = (e) => {
    setFolderName(e.target.value);
  };

  const handleUpload = () => {
    if (folderName === '' || selectedFiles.length === 0) {
      setError('Please provide a folder name and select files to upload.');
      return;
    }

    setError(null);
    setUploading(true);

    const formData = new FormData();
    formData.append('FolderName', folderName);

    for (let i = 0; i < selectedFiles.length; i++) {
      formData.append('Files', selectedFiles[i]);
    }

    axios
      .post('https://localhost:7074/api/pictures/upload', formData, {
        headers: {
          'Content-Type': 'multipart/form-data',
        },
      })
      .then((response) => {
        setUploadResponse(response.data);
        setProceedToCreate(true); // Proceed to create form after successful upload
      })
      .catch((err) => {
        setError('Error uploading pictures. Please try again later.');
      })
      .finally(() => {
        setUploading(false);
      });
  };

  return (
    <div>
      {proceedToCreate ? (
        <PictureCreateForm
          imageUrl={uploadResponse[0].url} // Assuming the first URL is the one you want to use
        />
      ) : (
        <div>
          <h2>Upload Pictures</h2>
          <div>
            <label>Folder Name:</label>
            <input type="text" value={folderName} onChange={handleFolderNameChange} />
          </div>
          <div>
            <label>Select Pictures:</label>
            <input type="file" multiple onChange={handleFileChange} />
          </div>
          <button onClick={handleUpload} disabled={uploading}>
            Upload
          </button>
          {error && <p className="text-red-600">{error}</p>}
          {uploadResponse && (
            <div>
              <h3>Upload Response:</h3>
              <pre>{JSON.stringify(uploadResponse, null, 2)}</pre>
            </div>
          )}
        </div>
      )}
    </div>
  );
}

export default PictureUploadForm;
