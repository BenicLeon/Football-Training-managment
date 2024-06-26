import React, { useState, useEffect } from 'react';
import axios from 'axios';
import UpdatePlayer from './UpdatePlayer';
import AddPlayer from './AddPlayer';
import FilterForm from './FilterForm';

const PlayerList = () => {
  const [players, setPlayers] = useState([]);
  const [selectedPlayer, setSelectedPlayer] = useState(null);
  const [showFilter, setShowFilter] = useState(false);
  const [filterOptions, setFilterOptions] = useState({
    positions: [],
    nationalities: [],
  });
  const [noPlayersMessage, setNoPlayersMessage] = useState('');
  const [filters, setFilters] = useState({
    Name: '',
    Position: '',
    Nationality: '',
    OrderBy: 'Name',
    OrderDirection: 'ASC',
    PageSize: 10,
    PageNumber: 1,
  });

  useEffect(() => {
    fetchPlayers();
  }, []);

  useEffect(() => {
    fetchFilteredPlayers(filters);
  }, [filters]);

  const fetchPlayers = async () => {
    try {
      const response = await axios.get('https://localhost:7070/Team/GetAllPlayers');
      const playersData = response.data;
      setPlayers(playersData);

      
      const positions = [...new Set(playersData.map(player => player.position))];
      const nationalities = [...new Set(playersData.map(player => player.nationality))];

      setFilterOptions({
        positions,
        nationalities,
      });

      console.log('Players fetched:', playersData);
    } catch (error) {
      console.error('Error fetching players:', error);
    }
  };

  const fetchFilteredPlayers = async (filters) => {
    try {
      const response = await axios.get('https://localhost:7070/Team/GetFilteredSortedPagedPlayers', {
        params: filters,
      });
      const filteredPlayers = response.data.data;
      if (filteredPlayers.length === 0) {
        setNoPlayersMessage('No players found for the selected filters.');
        setPlayers([]);
      } else {
        setNoPlayersMessage('');
        setPlayers(filteredPlayers);
      }
    } catch (error) {
      console.error('Error fetching filtered players:', error);
      setNoPlayersMessage('Error fetching filtered players.');
      setPlayers([]);
    }
  };

  const handleUpdate = (player) => {
    setSelectedPlayer(player);
  };

  const handleDelete = async (playerId) => {
    try {
      await axios.delete('https://localhost:7070/Team/DeletePlayer', { params: { id: playerId } });
      console.log('Player deleted:', playerId);
      setPlayers(players.filter(player => player.id !== playerId));
    } catch (error) {
      console.error('Error deleting player:', error);
    }
  };

  const handlePlayerUpdated = (updatedPlayer) => {
    setPlayers(players.map(player => player.id === updatedPlayer.id ? updatedPlayer : player));
    setSelectedPlayer(null);
  };

  const addPlayer = (newPlayer) => {
    console.log('Adding new player:', newPlayer);
    fetchPlayers(); 
  };

  const handleFilterChange = (updatedFilters) => {
    setFilters({ ...filters, ...updatedFilters, PageNumber: 1 });
  };

  const handlePageChange = (newPageNumber) => {
    setFilters({ ...filters, PageNumber: newPageNumber });
  };

  return (
    <div className="container">
      <AddPlayer onAddPlayer={addPlayer} />
      <h2>Player List</h2>
      <button onClick={() => setShowFilter(true)}>Filter</button>
      {showFilter && (
        <FilterForm 
          onClose={() => setShowFilter(false)} 
          onFilter={handleFilterChange}
          filterOptions={filterOptions}
        />
      )}
      {noPlayersMessage ? (
        <h1>{noPlayersMessage}</h1>
      ) : (
        <div>
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
                    <button className='update' onClick={() => handleUpdate(player)}>Update</button>
                    <button className='delete' onClick={() => handleDelete(player.id)}>Delete</button>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
          <div>
            <button onClick={() => handlePageChange(filters.PageNumber - 1)} disabled={filters.PageNumber === 1}>
              Previous
            </button>
            <button onClick={() => handlePageChange(filters.PageNumber + 1)}>
              Next
            </button>
          </div>
        </div>
      )}
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
