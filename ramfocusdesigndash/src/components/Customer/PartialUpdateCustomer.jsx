// PartialUpdateBooking.js
import React, { useState } from 'react';
import axios from 'axios';
import { applyOperation } from 'fast-json-patch';

function PartialUpdateCustomer({ customerId, onSuccess }) {
  const [formData, setFormData] = useState({
    FirstName: '',
  });

  const handleSubmit = (e) => {
    e.preventDefault();
    const patchDocument = [ {
        op: 'replace', path: '/FirstName', value: formData.FirstName },
    ];
    axios.patch(`https://localhost:7271/api/HotelBooking/UpdatePartial/${customerId}`, patchDocument)
      .then((response) => {
        console.log('customer partially updated successfully:', response.data);
        // Trigger a callback (onSuccess) or handle the success as needed
        
        if (onSuccess) {
          onSuccess(response.data);
        }
      })
      .catch((error) => {
        console.error('Error customer booking partially:', error);
        // Handle the error and display an error message
      });
  };

  return (
    <form onSubmit={handleSubmit} className="p-4">
      <div className="mb-4">
        <label className="block text-gray-700 text-sm font-bold mb-2" htmlFor="FirstName">
          First Name
        </label>
        <input
          className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
          id="FirstName"
          type="text"
          name="FirstName"
          value={formData.FirstName}
          onChange={(e) => setFormData({ FirstName: e.target.value })}
          required
        />
      </div>
      <div className="flex items-center justify-center">
        <button
          className="bg-blue-500 hover-bg-blue-700 text-white font-bold py-2 px-4 rounded focus:outline-none focus:shadow-outline"
          type="submit"
        >
          Update 
        </button>
      </div>
    </form>
  );
}

export default PartialUpdateCustomer;
