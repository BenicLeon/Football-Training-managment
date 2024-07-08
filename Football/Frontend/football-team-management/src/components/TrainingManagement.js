import React, { useEffect, useState } from 'react';
import axios from 'axios';
import { Link } from 'react-router-dom';
import '../styles/Style.css'; 

const TrainingManagement = () => {
    const [sessions, setSessions] = useState([]);
    const [newSession, setNewSession] = useState({ date: '', description: '' });
    const [isModalOpen, setIsModalOpen] = useState(false);
    const [currentSession, setCurrentSession] = useState(null);
    const [errorMessage, setErrorMessage] = useState('');

    useEffect(() => {
        fetchSessions();
    }, []);

    const fetchSessions = async () => {
        try {
            const token = localStorage.getItem('token');
            const response = await axios.get('/api/Training/sessions', {
                headers: {
                    Authorization: `Bearer ${token}`
                }
            });
            setSessions(response.data);
        } catch (error) {
            console.error('Error fetching training sessions', error);
        }
    };

    const handleAddSession = async () => {
        if (!newSession.date || !newSession.description) {
            setErrorMessage('Both date and description are required.');
            return;
        }

        try {
            const token = localStorage.getItem('token');
            const formattedSession = {
                date: newSession.date,
                description: newSession.description
            };
            console.log('Adding session with data:', formattedSession);

            const response = await axios.post('/api/Training/sessions', formattedSession, {
                headers: {
                    Authorization: `Bearer ${token}`,
                    'Content-Type': 'application/json'
                }
            });

            if (response.status === 201 || response.status === 200) {
                setNewSession({ date: '', description: '' });
                setErrorMessage('');
                fetchSessions();
            } else {
                console.error('Failed to add session', response);
            }
        } catch (error) {
            console.error('Error adding training session', error);
            if (error.response) {
                console.log('Error response data:', error.response.data);
                console.log('Error response status:', error.response.status);
                console.log('Error response headers:', error.response.headers);
            }
        }
    };

    const handleDeleteSession = async (id) => {
        try {
            const token = localStorage.getItem('token');
            await axios.delete(`/api/Training/sessions/${id}`, {
                headers: {
                    Authorization: `Bearer ${token}`
                }
            });
            fetchSessions();
        } catch (error) {
            console.error('Error deleting training session', error);
        }
    };

    const handleUpdateSession = async () => {
        try {
            const token = localStorage.getItem('token');
            await axios.put(`/api/Training/sessions/${currentSession.id}`, currentSession, {
                headers: {
                    Authorization: `Bearer ${token}`
                }
            });
            setIsModalOpen(false);
            fetchSessions();
        } catch (error) {
            console.error('Error updating training session', error);
        }
    };

    const openModal = (session) => {
        setCurrentSession(session);
        setIsModalOpen(true);
    };

    const closeModal = () => {
        setIsModalOpen(false);
        setCurrentSession(null);
    };

    return (
        <div className="training-management-container">
            <div className="training-management">
                <h1>Training Management</h1>
                <div className="add-session-form">
                    <input
                        type="datetime-local"
                        value={newSession.date}
                        onChange={(e) => setNewSession({ ...newSession, date: e.target.value })}
                    />
                    <input
                        type="text"
                        placeholder="Description"
                        value={newSession.description}
                        onChange={(e) => setNewSession({ ...newSession, description: e.target.value })}
                    />
                    <button className="btn-add" onClick={handleAddSession}><i className="fas fa-plus"></i></button>
                </div>
                {errorMessage && <p style={{ color: 'red' }}>{errorMessage}</p>}
                <ul>
                    {sessions.map((session) => (
                        <li key={session.id}>
                            {session.date} - {session.description}
                            <div>
                                <Link className="link-details" to={`/trainer/trainings/${session.id}`}>View Details</Link>
                                <button className="btn-update" onClick={() => openModal(session)}><i className="fas fa-edit"></i></button>
                                <button className="btn-delete" onClick={() => handleDeleteSession(session.id)}><i className="fas fa-trash-alt"></i></button>
                            </div>
                        </li>
                    ))}
                </ul>
            </div>
            {isModalOpen && (
                <div className="modal">
                    <div className="modal-content">
                        <h2>Update Training Session</h2>
                        <input
                            type="datetime-local"
                            value={currentSession.date}
                            onChange={(e) => setCurrentSession({ ...currentSession, date: e.target.value })}
                        />
                        <input
                            type="text"
                            placeholder="Description"
                            value={currentSession.description}
                            onChange={(e) => setCurrentSession({ ...currentSession, description: e.target.value })}
                        />
                        <button className="btn-save" onClick={handleUpdateSession}>Save Changes</button>
                        <button className="btn-cancel" onClick={closeModal}>Cancel</button>
                    </div>
                </div>
            )}
        </div>
    );
};

export default TrainingManagement;
