import React, { useState, useEffect } from 'react';
import axios from '../axios';
import UpdatePlayer from './UpdatePlayer';
import AddPlayer from './AddPlayer';


const PlayerList = () => {
  const [players, setPlayers] = useState([]);
  const [selectedPlayer, setSelectedPlayer] = useState(null);
  const [filter, setFilter] = useState({ name: '', position: '', nationality: '' });
  const [paging, setPaging] = useState({ pageSize: 10, pageNumber: 1 });
  const [sort, setSort] = useState({ orderBy: 'name', orderDirection: 'asc' });

  useEffect(() => {
    fetchPlayers();
  }, [filter, paging, sort]);

  const fetchPlayers = () => {
    axios.post('https://localhost:7070/Team/GetFilteredSortedPagedPlayers', { filter, paging, sort })
      .then(response => {
        setPlayers(response.data.data);
        console.log('Players fetched:', response.data.data);
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
        fetchPlayers(); 
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

  const handleFilterChange = (e) => {
    const { name, value } = e.target;
    setFilter(prevFilter => ({ ...prevFilter, [name]: value }));
  };

  const handlePageChange = (e) => {
    const { name, value } = e.target;
    setPaging(prevPaging => ({ ...prevPaging, [name]: parseInt(value) }));
  };

  const handleSortChange = (e) => {
    const { name, value } = e.target;
    setSort(prevSort => ({ ...prevSort, [name]: value }));
  };

  return (
    <div className="container">
      <AddPlayer onAddPlayer={addPlayer} />
      <h2>Player List</h2>

      <div className="filters">
        <input type="text" name="name" placeholder="Filter by Name" onChange={handleFilterChange} />
        <input type="text" name="position" placeholder="Filter by Position" onChange={handleFilterChange} />
        <input type="text" name="nationality" placeholder="Filter by Nationality" onChange={handleFilterChange} />
      </div>

      <div className="paging">
        <input type="number" name="pageSize" placeholder="Page Size" onChange={handlePageChange} />
        <input type="number" name="pageNumber" placeholder="Page Number" onChange={handlePageChange} />
      </div>

      <div className="sorting">
        <select name="orderBy" onChange={handleSortChange}>
          <option value="name">Name</option>
          <option value="age">Age</option>
        </select>
        <select name="orderDirection" onChange={handleSortChange}>
          <option value="asc">Ascending</option>
          <option value="desc">Descending</option>
        </select>
      </div>

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
            <tr key={player.id}>
              <td>{player.name}</td>
              <td>{player.position}</td>
              <td>{player.number}</td>
              <td>{player.age}</td>
              <td>{player.nationality}</td>
              <td>
                <button onClick={() => handleUpdate(player)}>Update</button>
                <button onClick={() => handleDelete(player.id)}>Delete</button>
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
