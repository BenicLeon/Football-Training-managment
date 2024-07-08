import React, { useEffect, useState } from 'react';
import axios from 'axios';

const AllPlayersWithContracts = () => {
    const [players, setPlayers] = useState([]);

    useEffect(() => {
        fetchPlayers();
    }, []);

    const fetchPlayers = async () => {
        try {
            const token = localStorage.getItem('token');
            const response = await axios.get('/api/Football/teamPlayers', {
                headers: {
                    Authorization: `Bearer ${token}`
                }
            });
            setPlayers(response.data.filter(player => player.contract && player.contract.trim() !== ''));
        } catch (error) {
            console.error('Error fetching players', error);
        }
    };

    return (
        <div>
            <h1>Players In the club</h1>
            <ul>
                {players.map((player) => (
                    <li key={player.id}>
                        {player.name} - {player.contract}
                    </li>
                ))}
            </ul>
        </div>
    );
};

export default AllPlayersWithContracts;
