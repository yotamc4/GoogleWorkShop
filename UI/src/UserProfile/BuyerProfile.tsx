import * as React from "react";
import * as BuyersControllerServices from "../Services/BuyersControllerServices";

import { Pivot, PivotItem } from "office-ui-fabric-react/lib/Pivot";
import { buttonHeaderProps } from "./UserProfileStyles";
import {
  FontWeights,
  Separator,
  Spinner,
  Stack,
  StackItem,
  Text,
} from "@fluentui/react";
import { GroupsList } from "./GroupsList";
import { Bid } from "../Modal/GroupDetails";
import { useAuth0 } from "@auth0/auth0-react";
import ButtonAppBar from "../LoginBar";

export const BuyerProfile: React.FunctionComponent = () => {
  const [groupsUserMemberIn, setGroupsUserMemberIn] = React.useState<Bid[]>();
  const [groupsCreatedByTheUser, setGroupsCreatedByTheUser] = React.useState<
    Bid[]
  >();

  const { getAccessTokenSilently } = useAuth0();

  async function updateCurrentGroups() {
    try {
      let [groupsCreatedByTheUser, groupsUserMemberIn] = await Promise.all([
        BuyersControllerServices.GetBidsCreatedByBuyer(getAccessTokenSilently),
        BuyersControllerServices.GetGroupsBuyerIsParticipant(
          getAccessTokenSilently
        ),
      ]);
      setGroupsCreatedByTheUser(groupsCreatedByTheUser.bidsPage);
      setGroupsUserMemberIn(groupsUserMemberIn.bidsPage);
    } catch {
      setGroupsCreatedByTheUser([]);
      setGroupsUserMemberIn([]);
    }
  }

  React.useEffect(() => {
    updateCurrentGroups();
  }, []);

  return (
    <Stack horizontalAlign="center">
      <ButtonAppBar />
      <StackItem>
        <Separator /> <Text variant={"large"}>Account type: </Text>
        <Text
          variant={"large"}
          styles={{ root: { fontWeight: FontWeights.bold } }}
        >
          {" "}
          Buyer
        </Text>
        <Separator />
      </StackItem>
      <Pivot styles={{ root: { marginBottom: "2rem" } }}>
        <PivotItem
          headerButtonProps={{
            text: "Groups I'm in",
            styles: buttonHeaderProps,
          }}
        >
          {groupsUserMemberIn ? (
            <GroupsList groups={groupsUserMemberIn} />
          ) : (
            <Spinner />
          )}
        </PivotItem>
        <PivotItem
          headerButtonProps={{
            text: "Groups created by me",
            styles: buttonHeaderProps,
          }}
        >
          {groupsCreatedByTheUser ? (
            <GroupsList groups={groupsCreatedByTheUser} />
          ) : (
            <Spinner />
          )}
        </PivotItem>
      </Pivot>
    </Stack>
  );
};
