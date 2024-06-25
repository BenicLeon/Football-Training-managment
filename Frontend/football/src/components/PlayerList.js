import React, { useState, useEffect } from 'react';
import axios from '../axios';
import UpdatePlayer from './UpdatePlayer';
import AddPlayer from './AddPlayer';

const PlayerList = () => {
  const [players, setPlayers] = useState([]);
  const [selectedPlayer, setSelectedPlayer] = useState(null);

  useEffect(() => {
    fetchPlayers();
  }, []);

  const fetchPlayers = () => {
    axios.get('https://localhost:7070/Team/GetAllPlayers')
      .then(response => {
        setPlayers(response.data);
        console.log('Players fetched:', response.data);
      })
      .catch(error => console.error('Error fetching players:', error));
  };

  const handleUpdate = (player) => {
    setSelectedPlayer(player);
  };

  const handleDelete = (playerId) => {
    axios.delete('https://localhost:7070/Team/DeletePlayer', { params: { id: playerId } })
      .then(() => {
        console.log('Player deleted:', playerId);
        setPlayers(players.filter(player => player.id !== playerId));
      })
      .catch(error => console.error('Error deleting player:', error));
  };

  const handlePlayerUpdated = (updatedPlayer) => {
    setPlayers(players.map(player => player.id === updatedPlayer.id ? updatedPlayer : player));
    setSelectedPlayer(null);
  };

  const addPlayer = (newPlayer) => {
    console.log('Adding new player:', newPlayer);
    fetchPlayers(); 
  };

  return (
    <div className="container">
      <AddPlayer onAddPlayer={addPlayer} />
      <h2>Player List</h2>
      <table>
        <thead>
          <tr>
            <th>Name</th>
            <th>Position</th>
            <th>Number</th>
            <th>Age</th>
            <th>Nationality</th>
            <th>Actions</th>
          </tr>
        </thead>
        <tbody>
          {players.map(player => (
            <tr key={player.id}> {/* Ensure unique key */}
              <td>{player.name}</td>
              <td>{player.position}</td>
              <td>{player.number}</td>
              <td>{player.age}</td>
              <td>{player.nationality}</td>
              <td>
                <button className='update' onClick={() => handleUpdate(player)}>Update</button>
                <button className='delete' onClick={() => handleDelete(player.id)}>Delete</button>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
      {selectedPlayer && (
        <UpdatePlayer 
          player={selectedPlayer} 
          onClose={() => setSelectedPlayer(null)} 
          onPlayerUpdated={handlePlayerUpdated}
        />
      )}
    </div>
  );
};

export default PlayerList;
