import React from 'react';

const Content = ({ list, handleCheck, handleDelete }) => {
  return (
    <main className="content">
      {list.length ? (
        <ul>
          {list.map((player) => (
            <li className="list" key={player.id}>
              <input
                type="checkbox"
                checked={player.checked}
                onChange={() => handleCheck(player.id)}
              />
              <label>
                <h3>{player.player}</h3>
              </label>
              <button onClick={() => handleDelete(player.id)}>X</button>
            </li>
          ))}
        </ul>
      ) : (
        <h3 style={{ marginTop: '2rem' }}>Empty List</h3>
      )}
    </main>
  );
};

export default Content;
