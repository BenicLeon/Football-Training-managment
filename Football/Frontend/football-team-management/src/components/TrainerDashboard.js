import React from 'react';
import { Link, Route, Routes, Navigate } from 'react-router-dom';
import TrainingManagement from './TrainingManagement';
import TrainingSessionDetails from './TrainingSessionDetails';
import PlayerPrivateTrainings from './PlayerPrivateTrainings';
import { AuthContext } from '../context/AuthContext';
import '../styles/Dashboard.css';

const TrainerDashboard = () => {
    const { logout } = React.useContext(AuthContext);

    return (
        <div className="dashboard-container">
            <nav>
                <ul>
                    <li>
                        <Link to="team">Manage Trainings</Link>
                    </li>
                    <li><Link to="/change-password">Change Password</Link></li>
                    <li>
                        <button onClick={logout}>Logout</button>
                    </li>
                </ul>
            </nav>
            <div className="dashboard-content">
                <Routes>
                    <Route path="/" element={<Navigate to="team" />} />
                    <Route path="team" element={<TrainingManagement />} />
                    <Route path="trainings/:id" element={<TrainingSessionDetails />} />
                    <Route path="players/:playerId/private-trainings" element={<PlayerPrivateTrainings />} />
                </Routes>
            </div>
        </div>
    );
};

export default TrainerDashboard;
