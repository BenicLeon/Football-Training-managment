import React from 'react';
import { Link, Route, Routes, Navigate } from 'react-router-dom';
import PlayerManagement from './PlayerManagement';
import UserManagement from './UserManagement'; 
import { AuthContext } from '../context/AuthContext';
import '../styles/Dashboard.css';

const SecretaryDashboard = () => {
    const { logout } = React.useContext(AuthContext);

    return (
        <div className="dashboard-container">
            <nav>
                <ul>
                    <li>
                        <Link to="players">Manage Players</Link>
                    </li>
                    <li>
                        <Link to="users">Manage Users</Link> 
                    </li>
                    <li>
                        <Link to="/change-password">Change Password</Link>
                    </li>
                    <li>
                        <button onClick={logout}>Logout</button>
                    </li>
                </ul>
            </nav>
            <div className="dashboard-content">
                <Routes>
                    <Route path="/" element={<Navigate to="players" />} />
                    <Route path="players" element={<PlayerManagement />} />
                    <Route path="users" element={<UserManagement />} />
                </Routes>
            </div>
        </div>
    );
};

export default SecretaryDashboard;
