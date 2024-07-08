import React, { useContext } from 'react';
import { Link } from 'react-router-dom';

import { AuthContext } from '../context/AuthContext';
import '../styles/Dashboard.css'; 

const Home = () => {
    const { user, logout } = useContext(AuthContext);

    const renderNavBar = () => {
        if (!user) return null;

        if (user.role === 'Secretary') {
            return (
                <nav>
                    <ul>
                        <li>
                            <Link to="/players">Manage Players</Link>
                        </li>
                        <li>
                            <Link to="/change-password">Change Password</Link>
                        </li>
                        <li>
                            <button onClick={logout}>Logout</button>
                        </li>
                    </ul>
                </nav>
            );
        }

        if (user.role === 'Coach') {
            return (
                <nav>
                    <ul>
                        <li>
                            <Link to="/team">Manage Trainings</Link>
                        </li>
                        <li>
                            <Link to="/change-password">Change Password</Link>
                        </li>
                        <li>
                            <button onClick={logout}>Logout</button>
                        </li>
                    </ul>
                </nav>
            );
        }

        if (user.role === 'Player') {
            return (
                <nav>
                    <ul>
                        
                        <li>
                            <Link to="/change-password">Change Password</Link>
                        </li>
                        <li>
                            <button onClick={logout}>Logout</button>
                        </li>
                    </ul>
                </nav>
            );
        }

        return null;
    };

    return (
        <div >
            {renderNavBar()}
            
                
           
        </div>
    );
};

export default Home;
