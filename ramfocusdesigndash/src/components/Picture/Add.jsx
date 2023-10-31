import React, { useState } from 'react';
import PictureUploadForm from './PictureUploadForm';
import PictureCreateForm from './PictureCreateForm'; // Adjust the path based on your project structure


function Add() {
  const [imageUrl, setImageUrl] = useState('');
  const [uploadSuccess, setUploadSuccess] = useState(false);
  const [error, setError] = useState(null);

  const handleUploadSuccess = (url) => {
    setImageUrl(url);
    setUploadSuccess(true);
  };

  const handleUploadError = (error) => {
    setError('An error occurred while uploading the picture.');
  };

  return (
    <div className="container mx-auto mt-5">
      {uploadSuccess ? (
        <PictureCreateForm
          imageUrl={imageUrl}
          onCreateSuccess={(data) => {
            // Handle successful creation
          }}
          onCreateError={(error) => {
            // Handle creation error
          }}
        />
      ) : (
        <PictureUploadForm
          onUploadSuccess={handleUploadSuccess}
          onUploadError={handleUploadError}
        />
      )}
    </div>
  );
}

export default Add;
