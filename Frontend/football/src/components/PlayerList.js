import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { useNavigate } from 'react-router-dom';
import FilterForm from './FilterForm';
import '../App.css';

const PlayerList = () => {
  const [players, setPlayers] = useState([]);
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
  const navigate = useNavigate();

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
      if (filteredPlayers && filteredPlayers.length === 0) {
        setNoPlayersMessage('No players found for the selected filters.');
        setPlayers([]);
      } else if (filteredPlayers) {
        setNoPlayersMessage('');
        setPlayers(filteredPlayers);
      } else {
        setNoPlayersMessage('No players found for the selected filters.');
        setPlayers([]);
      }
    } catch (error) {
      console.error('Error fetching filtered players:', error);
      setNoPlayersMessage('Error fetching filtered players.');
      setPlayers([]);
    }
  };

  const handleUpdate = (player) => {
    navigate(`/edit-player/${player.id}`);
  };

  const handleDelete = async (playerId) => {
    if (window.confirm('Are you sure you want to delete this player?')) {
      try {
        await axios.delete('https://localhost:7070/Team/DeletePlayer', { params: { id: playerId } });
        console.log('Player deleted:', playerId);
        setPlayers(players.filter(player => player.id !== playerId));
      } catch (error) {
        console.error('Error deleting player:', error);
      }
    }
  };

  const handleFilterChange = (updatedFilters) => {
    setFilters({ ...filters, ...updatedFilters, PageNumber: 1 });
  };

  return (
    <div className="container">

      <button className="filter" onClick={() => setShowFilter(true)}>Filter</button>
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
                <th style={{ width: '50px' }}>Number</th>
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
                    <button className='update' onClick={() => handleUpdate(player)}>
                      <i className="fas fa-edit"></i>
                    </button>
                    <button className='delete' onClick={() => handleDelete(player.id)}>
                      <i className="fas fa-trash-alt"></i>
                    </button>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      )}
    </div>
  );
};

export default PlayerList;
