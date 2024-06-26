import React from 'react';
import '../App.css';
import homeImage from '../assets/homeImage.png';

const Home = () => {
  return (
    <div className="home-page">
      <img src={homeImage} alt="Football" />
      <p>Manage your football team players with ease using our system.</p>
    </div>
  );
};

export default Home;
