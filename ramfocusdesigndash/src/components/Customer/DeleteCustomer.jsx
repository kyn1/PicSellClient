import React, { useState } from 'react';
import Modal from 'react-modal';
import axios from 'axios';

const API_KEY = 'your_api_key';
const API_SECRET = 'your_api_secret';
const HUBTEL_PAYMENT_URL = 'https://api.hubtel.com/v2/pos/onlinecheckout/items/initiate';

function CardPaymentForm({ paymentDetails, setPaymentDetails }) {
  return (
    <div>
      <label>Card Number:</label>
      <input
        type="text"
        value={paymentDetails.cardNumber}
        onChange={(e) => setPaymentDetails({ ...paymentDetails, cardNumber: e.target.value })}
      />
      <label>Expiration Date:</label>
      <input
        type="text"
        value={paymentDetails.expirationDate}
        onChange={(e) => setPaymentDetails({ ...paymentDetails, expirationDate: e.target.value })}
      />
      <label>CVV:</label>
      <input
        type="text"
        value={paymentDetails.cvv}
        onChange={(e) => setPaymentDetails({ ...paymentDetails, cvv: e.target.value })}
      />
    </div>
  );
}

function MobileMoneyForm({ paymentDetails, setPaymentDetails }) {
  return (
    <div>
      <label>Mobile Number:</label>
      <input
        type="text"
        value={paymentDetails.mobileNumber}
        onChange={(e) => setPaymentDetails({ ...paymentDetails, mobileNumber: e.target.value })}
      />
    </div>
  );
}

function PaymentMode() {
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [paymentMethod, setPaymentMethod] = useState(''); // 'card' or 'mobileMoney'
  const [paymentData, setPaymentData] = useState({});
  const [paymentDetails, setPaymentDetails] = useState({
    card: {
      cardNumber: '',
      expirationDate: '',
      cvv: '',
    },
    mobileMoney: {
      mobileNumber: '',
    },
  });

  const handlePayment = () => {
    setIsModalOpen(true);
  };

  const closeModal = () => {
    setIsModalOpen(false);
  };

  const handlePaymentMethodChange = (method) => {
    setPaymentMethod(method);
  };

  const handlePaymentSubmit = async () => {
    try {
      const response = await axios.post(HUBTEL_PAYMENT_URL, {
        apiKey: API_KEY,
        apiSecret: API_SECRET,
        paymentDetails: paymentDetails[paymentMethod],
      });
      setPaymentData(response.data);
      closeModal();
    } catch (error) {
      console.error('Error initiating payment:', error);
    }
  };

  return (
    <div>
      <button onClick={handlePayment}>Pay with Hubtel</button>
      <Modal
        isOpen={isModalOpen}
        onRequestClose={closeModal}
        contentLabel="Payment Modal"
      >
        <h2>Payment Details</h2>
        <div>
          <label>
            <input
              type="radio"
              name="paymentMethod"
              value="card"
              checked={paymentMethod === 'card'}
              onChange={() => handlePaymentMethodChange('card')}
            />
            Credit Card
          </label>
          <label>
            <input
              type="radio"
              name="paymentMethod"
              value="mobileMoney"
              checked={paymentMethod === 'mobileMoney'}
              onChange={() => handlePaymentMethodChange('mobileMoney')}
            />
            Mobile Money
          </label>
        </div>
        <form>
          {paymentMethod === 'card' && <CardPaymentForm paymentDetails={paymentDetails.card} setPaymentDetails={setPaymentDetails.card} />}
          {paymentMethod === 'mobileMoney' && <MobileMoneyForm paymentDetails={paymentDetails.mobileMoney} setPaymentDetails={setPaymentDetails.mobileMoney} />}
          <button type="button" onClick={handlePaymentSubmit}>Submit Payment</button>
        </form>
        <button onClick={closeModal}>Close</button>
      </Modal>
    </div>
  );
}

export default PaymentMode;
