import React, { useEffect, useState } from 'react';
import axios from 'axios';
import Modal from './Modal';
import '../styles/Style.css'; 


const PlayerManagement = () => {
    const [players, setPlayers] = useState([]);
    const [teamPlayers, setTeamPlayers] = useState([]);
    const [selectedPlayer, setSelectedPlayer] = useState(null);
    const [contract, setContract] = useState('');
    const [showModal, setShowModal] = useState(false);
    const contracts = ['Profesionalni', 'Amaterski', 'Juniorski', 'Kadetski', 'Pionirski', 'Početnički'];

    useEffect(() => {
        fetchPlayers();
        fetchTeamPlayers();
    }, []);

    const fetchPlayers = async () => {
        try {
            const token = localStorage.getItem('token');
            const response = await axios.get('/api/Football/players', {
                headers: {
                    Authorization: `Bearer ${token}`
                }
            });
            setPlayers(response.data);
        } catch (error) {
            console.error('Error fetching players', error);
        }
    };

    const fetchTeamPlayers = async () => {
        try {
            const token = localStorage.getItem('token');
            const response = await axios.get('/api/Football/teamPlayers', {
                headers: {
                    Authorization: `Bearer ${token}`
                }
            });
            setTeamPlayers(response.data);
        } catch (error) {
            console.error('Error fetching team players', error);
        }
    };

    const isPlayerInTeam = (playerId) => {
        return teamPlayers.some(player => player.id === playerId);
    };

    const addPlayerToTeam = async (playerId) => {
        try {
            const player = players.find(player => player.id === playerId);

            if (!player.contract || player.contract.trim() === '') {
                alert('Player must have a contract before being added to the team.');
                return;
            }

            if (isPlayerInTeam(playerId)) {
                alert('Player is already in the team.');
                return;
            }

            const token = localStorage.getItem('token');
            await axios.post('/api/Football/teamPlayers', { playerId }, {
                headers: {
                    Authorization: `Bearer ${token}`
                }
            });
            fetchTeamPlayers();
        } catch (error) {
            console.error('Error adding player to team', error);
        }
    };

    const removePlayerFromTeam = async (playerId) => {
        try {
            const token = localStorage.getItem('token');
            await axios.delete(`/api/Football/teamPlayers/${playerId}`, {
                headers: {
                    Authorization: `Bearer ${token}`
                }
            });
            fetchTeamPlayers();
        } catch (error) {
            console.error('Error removing player from team', error);
        }
    };

    const updatePlayerContract = async (e) => {
        e.preventDefault();
        try {
            const token = localStorage.getItem('token');
            await axios.put(`/api/Football/players/${selectedPlayer.id}/contract`, { contract }, {
                headers: {
                    Authorization: `Bearer ${token}`
                }
            });
            fetchPlayers();
            fetchTeamPlayers();
            setSelectedPlayer(null);
            setContract('');
            setShowModal(false);
        } catch (error) {
            console.error('Error updating player contract', error);
        }
    };

    const handleSetContract = (player) => {
        setSelectedPlayer(player);
        setContract(player.contract || '');
        setShowModal(true);
    };

    return (
        <div className="player-management-container">
            <div className="player-management">
                <h1>Manage Players</h1>
                <h2>All Players</h2>
                <ul>
                    {players.filter(player => !isPlayerInTeam(player.id)).map((player) => (
                        <li key={player.id}>
                            {player.name} - {player.contract}
                            <div>
                                <button className="btn-set-contract" onClick={() => handleSetContract(player)}>
                                    <i className="fas fa-edit"></i>
                                </button>
                                <button className="btn-add" onClick={() => addPlayerToTeam(player.id)}>
                                    <i className="fas fa-plus"></i>
                                </button>
                            </div>
                        </li>
                    ))}
                </ul>
                <h2>Team Players</h2>
                <ul>
                    {teamPlayers.map((player) => (
                        <li key={player.id}>
                            {player.name} - {player.contract}
                            <div>
                                <button className="btn-set-contract" onClick={() => handleSetContract(player)}>
                                    <i className="fas fa-edit"></i>
                                </button>
                                <button className="btn-remove" onClick={() => removePlayerFromTeam(player.id)}>
                                    <i className="fas fa-trash"></i>
                                </button>
                            </div>
                        </li>
                    ))}
                </ul>
            </div>
            {selectedPlayer && (
                <Modal show={showModal} handleClose={() => setShowModal(false)}>
                    <form onSubmit={updatePlayerContract}>
                        <h3>Edit Contract for {selectedPlayer.name}</h3>
                        <select
                            value={contract}
                            onChange={(e) => setContract(e.target.value)}
                            required
                        >
                            <option value="">Select Contract</option>
                            {contracts.map((contractOption) => (
                                <option key={contractOption} value={contractOption}>{contractOption}</option>
                            ))}
                        </select>
                        <button className='update-contract' type="submit">Update Contract</button>
                    </form>
                </Modal>
            )}
        </div>
    );
};

export default PlayerManagement;
