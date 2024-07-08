import React, { useEffect, useState } from 'react';
import axios from 'axios';
import { useParams, Link } from 'react-router-dom';
import '../styles/Style.css'; 

const PlayerPrivateTrainings = () => {
    const { playerId } = useParams();
    const [privateTrainings, setPrivateTrainings] = useState([]);

    useEffect(() => {
        const fetchPrivateTrainings = async () => {
            try {
                const token = localStorage.getItem('token');
                const response = await axios.get(`/api/Training/team-players/${playerId}/private-trainings`, {
                    headers: {
                        Authorization: `Bearer ${token}`
                    }
                });
                setPrivateTrainings(response.data);
            } catch (error) {
                console.error('Error fetching private trainings', error);
            }
        };

        fetchPrivateTrainings();
    }, [playerId]);

    return (
        <div className="private-trainings-container">
            <h1>Private Trainings</h1>
            <table className="private-trainings-table">
                <thead>
                    <tr>
                        <th>#</th>
                        <th>Link</th>
                    </tr>
                </thead>
                <tbody>
                    {privateTrainings.map((link, index) => (
                        <tr key={index}>
                            <td>{index + 1}</td>
                            <td>
                                <a href={link} target="_blank" rel="noopener noreferrer">{link}</a>
                            </td>
                        </tr>
                    ))}
                </tbody>
            </table>
            <p>
                        <Link to="/trainer">Go back</Link>
                    </p>
        </div>
    );
};

export default PlayerPrivateTrainings;
