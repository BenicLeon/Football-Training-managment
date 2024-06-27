import React from 'react';
import { Link } from 'react-router-dom';
import { useUser } from '../UserContext';

function Navbar() {
  const { user } = useUser();

  return (
    <nav>
      <div className='nav-container'>
        <h3>FOOTBALL PLAYERS</h3>
        <ul>
          <li><Link to="/">HOME</Link></li>
          <li><Link to="/players">PLAYERS</Link></li>
          <li><Link to="/add-player">ADD PLAYER</Link></li>
        </ul>
        <div className="user-info">
          {user ? `Welcome, ${user.name}` : 'Welcome, Guest'}
        </div>
      </div>
    </nav>
  );
}

export default Navbar;
