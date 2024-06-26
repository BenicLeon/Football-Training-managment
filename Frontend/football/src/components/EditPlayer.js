import React, { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import axios from 'axios';

const EditPlayer = () => {
  const { id } = useParams();
  const navigate = useNavigate();
  const [player, setPlayer] = useState(null);
  const [name, setName] = useState('');
  const [position, setPosition] = useState('');
  const [number, setNumber] = useState(0);
  const [age, setAge] = useState(0);
  const [nationality, setNationality] = useState('');

  useEffect(() => {
    const fetchPlayer = async () => {
      try {
        
        const response = await axios.get(`https://localhost:7070/Team/GetPlayerById/${id}`);
        const playerData = response.data;
        setPlayer(playerData);
        
        setName(playerData.name);
        setPosition(playerData.position);
        setNumber(playerData.number);
        setAge(playerData.age);
        setNationality(playerData.nationality);
      } catch (error) {
        console.error('Error fetching player:', error);
      }
    };
    fetchPlayer();
  }, [id]);

  const handleSubmit = async (event) => {
    event.preventDefault();
    const updatedPlayer = { id: player.id, name, position, number, age, nationality };
    try {
      await axios.put(`https://localhost:7070/Team/UpdatePlayer/${player.id}`, updatedPlayer);
      navigate('/players');
    } catch (error) {
      console.error('Error updating player:', error);
    }
  };

  if (!player) return <div>Loading...</div>;

  return (
    <div>
      <h2>Edit Player</h2>
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
        <button type="button" onClick={() => navigate('/players')}>Cancel</button>
      </form>
    </div>
  );
};

export default EditPlayer;
