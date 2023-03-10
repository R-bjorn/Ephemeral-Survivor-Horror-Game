using UnityEngine;
using Unity.Netcode;
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
	public class StarterAssetsInputs : NetworkBehaviour
	{
		[Header("Character Input Values")]
		public Vector2 move;
		public Vector2 look;
		public bool jump;
		public bool sprint;

		[Header("Movement Settings")]
		public bool analogMovement;

		[Header("Mouse Cursor Settings")]
		public bool cursorLocked = true;
		public bool cursorInputForLook = true;

#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
		public void OnMove(InputValue value)
		{
			if(!IsOwner) return;
			MoveInput(value.Get<Vector2>());
		}

		public void OnLook(InputValue value)
		{
			if(!IsOwner) return;
			if(cursorInputForLook)
			{
				LookInput(value.Get<Vector2>());
			}
		}

		public void OnJump(InputValue value)
		{
			if(!IsOwner) return;
			JumpInput(value.isPressed);
		}

		public void OnSprint(InputValue value)
		{
			if(!IsOwner) return;
			SprintInput(value.isPressed);
		}
#endif


		public void MoveInput(Vector2 newMoveDirection)
		{
			if (!IsOwner) return;
			move = newMoveDirection;
		} 

		public void LookInput(Vector2 newLookDirection)
		{
			if (!IsOwner) return;
			look = newLookDirection;
		}

		public void JumpInput(bool newJumpState)
		{
			if (!IsOwner) return;
			jump = newJumpState;
		}

		public void SprintInput(bool newSprintState)
		{
			if (!IsOwner) return;
			sprint = newSprintState;
		}

		private void OnApplicationFocus(bool hasFocus)
		{
			if (!IsOwner) return;
			SetCursorState(cursorLocked);
		}

		private void SetCursorState(bool newState)
		{
			if (!IsOwner) return;
			Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
		}
	}
	
}