import React, { useEffect, useState, useCallback } from 'react';
import axios from 'axios';
import { useParams, Link } from 'react-router-dom';
import '../styles/Style.css'; 

const TrainingSessionDetails = () => {
    const { id } = useParams();
    const [session, setSession] = useState(null);
    const [players, setPlayers] = useState([]);

    const fetchSessionDetails = useCallback(async () => {
        try {
            const token = localStorage.getItem('token');
            const response = await axios.get(`/api/Training/sessions/${id}`, {
                headers: {
                    Authorization: `Bearer ${token}`
                }
            });
            setSession(response.data);
        } catch (error) {
            console.error('Error fetching session details', error);
        }
    }, [id]);

    const fetchPlayers = useCallback(async () => {
        try {
            const token = localStorage.getItem('token');
            const response = await axios.get(`/api/Training/sessions/${id}/players`, {
                headers: {
                    Authorization: `Bearer ${token}`
                }
            });

            const playersData = await Promise.all(response.data.map(async player => {
                const privateTrainingsResponse = await axios.get(`/api/Training/team-players/${player.id}/private-trainings`, {
                    headers: {
                        Authorization: `Bearer ${token}`
                    }
                });
                return {
                    ...player,
                    hasPrivateTrainings: privateTrainingsResponse.data.length > 0
                };
            }));

            setPlayers(playersData);
        } catch (error) {
            console.error('Error fetching players', error);
        }
    }, [id]);

    useEffect(() => {
        fetchSessionDetails();
        fetchPlayers();
    }, [fetchSessionDetails, fetchPlayers]);

    return (
        <div className="training-session-details">
            {session && (
                <>
                    <h1>Training Session Details</h1>
                    <p>Date: {session.date}</p>
                    <p>Description: {session.description}</p>
                    <h2>Players Attending</h2>
                    <ul>
                        {players.map(player => (
                            <li key={player.id}>
                                {player.hasPrivateTrainings ? (
                                    <Link to={`/trainer/team-players/${player.id}/private-trainings`}>
                                        {player.name}
                                    </Link>
                                ) : (
                                    <span>{player.name}</span>
                                )}
                            </li>
                        ))}
                    </ul>
                    <p>
                        <Link to="/trainer">Go back</Link>
                    </p>
                </>
            )}
        </div>
    );
};

export default TrainingSessionDetails;
