import React, { useEffect, useState, useContext, useCallback } from 'react';
import axios from 'axios';
import { AuthContext } from '../context/AuthContext';
import { Link } from 'react-router-dom';
import '../styles/PlayerDashboard.css'; 

const PlayerDashboard = () => {
    const [sessions, setSessions] = useState([]);
    const [attendingSessions, setAttendingSessions] = useState([]);
    const [isInTeam, setIsInTeam] = useState(false);
    const [privateTrainingLink, setPrivateTrainingLink] = useState('');
    const [privateTrainings, setPrivateTrainings] = useState([]);
    const [errorMessage, setErrorMessage] = useState('');
    const { user, logout } = useContext(AuthContext);

    const checkTeamStatus = useCallback(async () => {
        try {
            const token = localStorage.getItem('token');
            const response = await axios.get(`/api/Football/teamStatus/${user.playerId}`, {
                headers: {
                    Authorization: `Bearer ${token}`
                }
            });
            setIsInTeam(response.data.isInTeam);
        } catch (error) {
            console.error('Error checking team status', error);
        }
    }, [user.playerId]);

    const fetchAttendingSessions = useCallback(async () => {
        try {
            const token = localStorage.getItem('token');
            const response = await axios.get(`/api/Training/my-sessions/${user.playerId}`, {
                headers: {
                    Authorization: `Bearer ${token}`
                }
            });
            setAttendingSessions(response.data.map(session => session.id));
        } catch (error) {
            console.error('Error fetching attending sessions', error);
        }
    }, [user.playerId]);

    const fetchSessions = useCallback(async () => {
        try {
            const token = localStorage.getItem('token');
            const response = await axios.get('/api/Training/sessions', {
                headers: {
                    Authorization: `Bearer ${token}`
                }
            });
            setSessions(response.data);
            fetchAttendingSessions();
        } catch (error) {
            console.error('Error fetching training sessions', error);
        }
    }, [fetchAttendingSessions]);

    const fetchPrivateTrainings = useCallback(async () => {
        try {
            const token = localStorage.getItem('token');
            const response = await axios.get(`/api/Training/team-players/${user.playerId}/private-trainings`, {
                headers: {
                    Authorization: `Bearer ${token}`
                }
            });
            setPrivateTrainings(response.data);
        } catch (error) {
            console.error('Error fetching private trainings', error);
        }
    }, [user.playerId]);

    useEffect(() => {
        checkTeamStatus();
    }, [checkTeamStatus]);

    useEffect(() => {
        if (isInTeam) {
            fetchSessions();
            fetchPrivateTrainings();
        }
    }, [isInTeam, fetchSessions, fetchPrivateTrainings]);

    const handleAttendSession = async (sessionId) => {
        try {
            const token = localStorage.getItem('token');
            await axios.post(`/api/Training/sessions/${sessionId}/attend`, { playerId: user.playerId }, {
                headers: {
                    Authorization: `Bearer ${token}`,
                    'Content-Type': 'application/json'
                }
            });
            fetchSessions();
        } catch (error) {
            console.error('Error attending session', error);
        }
    };

    const handleUnattendSession = async (sessionId) => {
        try {
            const token = localStorage.getItem('token');
            await axios.post(`/api/Training/sessions/${sessionId}/unattend`, { playerId: user.playerId }, {
                headers: {
                    Authorization: `Bearer ${token}`,
                    'Content-Type': 'application/json'
                }
            });
            fetchSessions();
        } catch (error) {
            console.error('Error unattending session', error);
        }
    };

    const handleAddPrivateTraining = async (e) => {
        e.preventDefault();
        
        const youtubeLinkPattern = /^https:\/\/www\.youtube\.com\/watch\?v=[\w-]+(&[\w-]+=[\w-]+)*$/;

        if (!youtubeLinkPattern.test(privateTrainingLink)) {
            setErrorMessage('Invalid format. Please enter a valid YouTube link.');
            return;
        }

        try {
            const token = localStorage.getItem('token');
            await axios.post(`/api/Training/team-players/${user.playerId}/add-private-training`, 
                privateTrainingLink, 
                {
                    headers: {
                        Authorization: `Bearer ${token}`,
                        'Content-Type': 'application/json'
                    }
                }
            );
            setPrivateTrainingLink('');
            setErrorMessage('');
            fetchPrivateTrainings();
        } catch (error) {
            console.error('Error adding private training', error);
        }
    };

    return (
        <div className="player-dashboard-container">
            {user && user.role === 'Player' ? (
                isInTeam ? (
                    <div className="player-dashboard-content">
                        <h1>Training Sessions</h1>
                        <ul className="player-dashboard-sessions">
                            {sessions.map((session) => (
                                <li key={session.id} className="player-dashboard-session-item">
                                    {session.date} - {session.description}
                                    {attendingSessions.includes(session.id) ? (
                                        <>
                                            <span className="player-dashboard-attending">✔ You are attending this session</span>
                                            <button className="player-dashboard-button-unattend" onClick={() => handleUnattendSession(session.id)}>Unattend</button>
                                        </>
                                    ) : (
                                        <button className="player-dashboard-button" onClick={() => handleAttendSession(session.id)}>Attend</button>
                                    )}
                                </li>
                            ))}
                        </ul>
                        <h2>Private Trainings</h2>
                        <ul className="player-dashboard-private-trainings">
                            {privateTrainings.map((link, index) => (
                                <li key={index} className="player-dashboard-private-training-item">
                                    <a href={link} target="_blank" rel="noopener noreferrer">{link}</a>
                                </li>
                            ))}
                        </ul>
                        <form onSubmit={handleAddPrivateTraining}>
                            <input 
                                type="text" 
                                value={privateTrainingLink} 
                                onChange={(e) => setPrivateTrainingLink(e.target.value)} 
                                placeholder="Add a private training link" 
                                required 
                            />
                            <button className="player-dashboard-button" type="submit"><i className="fas fa-plus"></i></button>
                        </form>
                        {errorMessage && <p className="error-message">{errorMessage}</p>}
                        <Link to="/change-password" className="player-dashboard-link">Change Password</Link>
                        <button className="player-dashboard-logout" onClick={logout}>Logout</button>
                    </div>
                ) : (
                    <div className="player-dashboard-content">
                        <h1>You don't have a contract with this team. Please wait for assignment.</h1>
                        <button className="player-dashboard-logout" onClick={logout}>Logout</button>
                    </div>
                )
            ) : (
                <div className="player-dashboard-content">
                    <h1>You are not authorized to view this page.</h1>
                    <button className="player-dashboard-logout" onClick={logout}>Logout</button>
                </div>
            )}
        </div>
    );
};

export default PlayerDashboard;
