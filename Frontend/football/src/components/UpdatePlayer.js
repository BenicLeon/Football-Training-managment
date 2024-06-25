import React, { useState, useEffect } from 'react';
import axios from '../axios';

const UpdatePlayer = ({ player, onClose, onPlayerUpdated }) => {
  const [name, setName] = useState(player.name);
  const [position, setPosition] = useState(player.position);
  const [number, setNumber] = useState(player.number);
  const [age, setAge] = useState(player.age);
  const [nationality, setNationality] = useState(player.nationality);

  useEffect(() => {
    setName(player.name);
    setPosition(player.position);
    setNumber(player.number);
    setAge(player.age);
    setNationality(player.nationality);
  }, [player]);

  const handleSubmit = (event) => {
    event.preventDefault();
    const updatedPlayer = { id: player.id, name, position, number, age, nationality };
    axios.put(`https://localhost:7070/Team/UpdatePlayer/${player.id}`, updatedPlayer)
      .then(response => {
        console.log('Player updated:', response.data);
        onPlayerUpdated(updatedPlayer);
      })
      .catch(error => console.error('Error updating player:', error));
  };

  return (
    <div>
      <h2>Update Player</h2>
      <form onSubmit={handleSubmit}>
        <input 
          type="text" 
          value={name} 
          onChange={(e) => setName(e.target.value)} 
          placeholder="Player Name" 
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
        <button type="submit">Update Player</button>
        <button type="button" onClick={onClose}>Cancel</button>
      </form>
    </div>
  );
};

export default UpdatePlayer;