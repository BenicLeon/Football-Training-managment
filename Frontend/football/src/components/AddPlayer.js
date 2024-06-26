import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import axios from '../axios';

const AddPlayer = ({ onAddPlayer }) => {
  const [name, setName] = useState('');
  const [position, setPosition] = useState('');
  const [number, setNumber] = useState(0);
  const [age, setAge] = useState(0);
  const [nationality, setNationality] = useState('');
  const navigate = useNavigate();

  const handleSubmit = async (event) => {
    event.preventDefault();
    const newPlayer = { name, position, number, age, nationality };
    console.log('Submitting player:', newPlayer);

    try {
      const response = await axios.post('https://localhost:7070/Team/AddPlayer', newPlayer);
      console.log('Player added:', response.data);
      onAddPlayer(response.data);

      // Clear the form fields
      setName('');
      setPosition('');
      setNumber(0);
      setAge(0);
      setNationality('');

      // Navigate back to the players list after adding
      navigate('/players');
    } catch (error) {
      console.error('Error adding player:', error);
    }
  };

  return (
    <div>
      <h2>Add Player</h2>
      <form onSubmit={handleSubmit}>
        <input 
          type="text" 
          value={name} 
          onChange={(e) => setName(e.target.value)} 
          placeholder="Full Name" 
          required 
        />
        <input 
          type="text" 
          value={position} 
          onChange={(e) => setPosition(e.target.value)} 
          placeholder="Position" 
          required 
        />
        <input 
          type="number" 
          value={number} 
          onChange={(e) => setNumber(Number(e.target.value))} 
          placeholder="Number" 
          required 
        />
        <input 
          type="number" 
          value={age} 
          onChange={(e) => setAge(Number(e.target.value))} 
          placeholder="Age" 
          required 
        />
        <input 
          type="text" 
          value={nationality} 
          onChange={(e) => setNationality(e.target.value)} 
          placeholder="Nationality" 
          required 
        />
        <button type="submit">Add Player</button>
      </form>
    </div>
  );
};

export default AddPlayer;
